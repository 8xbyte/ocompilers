#include <sessions.hpp>
#include <error.hpp>
#include <check_auth.hpp>
#include <config.hpp>

#include <boost/lexical_cast.hpp>
#include <boost/url.hpp>

#include <iostream>

namespace gateway {
    void http_session(
        boost::asio::ip::tcp::socket _socket,
        boost::beast::http::request<boost::beast::http::string_body> _request, 
        std::string _to,
        bool _isAuth
    ) {
        try {
            boost::system::error_code ec;
            boost::asio::io_context context;

            auto cors = gateway::config::app.get_object("ApiGateway").get_object("Http").get_object("Cors");
            auto allow_origin = cors.get_value("AllowOrigin").to_string();
            auto allow_methods = cors.get_value("AllowMethods").to_string();
            auto allow_headers = cors.get_value("AllowHeaders").to_string();
            auto allow_credentials = cors.get_value("AllowCredentials").to_string();

            auto to_url = boost::urls::url_view(_to);

            _request.target(std::format("{}?{}", to_url.path(), to_url.query()));

            if (_isAuth) {
                auto [isSuccess, userId] = gateway::check_auth(_request);
                if (!isSuccess) {
                    boost::beast::http::response<boost::beast::http::string_body> input_response(boost::beast::http::status::forbidden, _request.version());
                    input_response.set("Access-Control-Allow-Origin", allow_origin);
                    input_response.set("Access-Control-Allow-Methods", allow_methods);
                    input_response.set("Access-Control-Allow-Headers", allow_headers);
                    input_response.set("Access-Control-Allow-Credentials", allow_credentials);
                    input_response.body() = "403 Forbidden";
                    input_response.prepare_payload();
                    boost::beast::http::write(_socket, input_response, ec);
                    #if defined(DEBUG) || defined(_DEBUG)
                    IS_SYSTEM_ERROR(ec);
                    #endif

                    _socket.close(ec);
                    #if defined(DEBUG) || defined(_DEBUG)
                    IS_SYSTEM_ERROR(ec);
                    #endif

                    return;
                } else {
                    auto header_name = gateway::config::app.get_object("ApiGateway").get_object("Http").get_object("Headers").get_value("UserId").to_string();
                    _request.set(header_name, std::to_string(userId));
                }
            }


            boost::asio::ip::tcp::socket output_socket(context);
            output_socket.connect(boost::asio::ip::tcp::endpoint(
                boost::asio::ip::make_address(to_url.host()), boost::lexical_cast<uint16_t>(
                    to_url.port()
                )
            ), ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            boost::beast::http::write(output_socket, _request, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            boost::beast::flat_buffer output_buffer;
            boost::beast::http::response<boost::beast::http::string_body> output_response;
            boost::beast::http::read(output_socket, output_buffer, output_response, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            output_response.set("Access-Control-Allow-Origin", allow_origin);
            output_response.set("Access-Control-Allow-Methods", allow_methods);
            output_response.set("Access-Control-Allow-Headers", allow_headers);
            output_response.set("Access-Control-Allow-Credentials", allow_credentials);

            boost::beast::http::write(_socket, output_response, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            if (output_socket.is_open()) {
                output_socket.close(ec);
                #if defined(DEBUG) || defined(_DEBUG)
                IS_SYSTEM_ERROR(ec);
                #endif
            }

            if (_socket.is_open()) {
                _socket.close(ec);
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