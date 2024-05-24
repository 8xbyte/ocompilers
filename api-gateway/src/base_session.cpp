#include <sessions.hpp>
#include <config.hpp>
#include <error.hpp>

#include <boost/url.hpp>

#include <iostream>
#include <format>

namespace gateway {
    void base_session(boost::asio::ip::tcp::socket _socket) {
        try {
            boost::system::error_code ec;
            boost::beast::flat_buffer buffer;
            boost::beast::http::request<boost::beast::http::string_body> request;
            boost::beast::http::read(_socket, buffer, request, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            bool is_found = false;
            auto routes = gateway::config::app.get_object("ApiGateway").get_array("Routes");
            for (size_t i = 0; i < routes.size(); i++) {
                auto from = routes.get_object(i).get_value("From").to_string(), to = routes.get_object(i).get_value("To").to_string();
                auto is_auth = routes.get_object(i).get_value("IsAuth").to_bool();

                auto from_url = boost::urls::url_view(from);
                auto target_url = boost::urls::url_view(request.target());


                if (target_url.path() == from_url.path()) {
                    is_found = true;
                    if (request.method() == boost::beast::http::verb::options) {
                        std::cout << "Preflight request from " << from << " to " << to << std::endl;
                        std::thread(preflight_session, std::move(_socket), std::move(request)).detach();
                    } else if (boost::beast::websocket::is_upgrade(request)) {
                        std::cout << "Redirect websocket request from " << from << " to " << to << std::endl;
                        std::thread(websocket_session, std::move(_socket), std::move(request), std::format("{}?{}", to, target_url.query()), is_auth).detach();
                    } else {
                        std::cout << "Redirect http request from " << from << " to " << to << std::endl;
                        std::thread(http_session, std::move(_socket), std::move(request), std::format("{}?{}", to, target_url.query()), is_auth).detach();
                    }
                }
            }

            if (!is_found) {
                std::cout << "Not found route for " << request.target() << std::endl;
                boost::beast::http::response<boost::beast::http::string_body> response(boost::beast::http::status::not_found, request.version());
                response.body() = "404 Not found";
                response.prepare_payload();

                boost::beast::http::write(_socket, response, ec);
                #if defined(DEBUG) || defined(_DEBUG)
                IS_SYSTEM_ERROR(ec);
                #endif

                if (_socket.is_open()) _socket.close(ec);
                #if defined(DEBUG) || defined(_DEBUG)
                IS_SYSTEM_ERROR(ec);
                #endif
            }
        } catch (std::exception const& e) {
            #if defined(DEBUG) || defined(_DEBUG)
            EXCEPTION_ERROR(e);
            #endif
        }
    }
}