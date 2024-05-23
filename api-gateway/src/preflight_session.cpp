#include <sessions.hpp>
#include <error.hpp>
#include <config.hpp>

namespace gateway {
    void preflight_session(
        boost::asio::ip::tcp::socket _socket,
        boost::beast::http::request<boost::beast::http::string_body> _request
    ) {
        try {
            boost::system::error_code ec;

            auto cors = gateway::config::app.get_object("ApiGateway").get_object("Http").get_object("Cors");
            auto allow_origin = cors.get_value("AllowOrigin").to_string();
            auto allow_methods = cors.get_value("AllowMethods").to_string();
            auto allow_headers = cors.get_value("AllowHeaders").to_string();
            auto allow_credentials = cors.get_value("AllowCredentials").to_string();

            boost::beast::http::response<boost::beast::http::empty_body> response(boost::beast::http::status::no_content, _request.version());
            response.set("Access-Control-Allow-Origin", allow_origin);
            response.set("Access-Control-Allow-Methods", allow_methods);
            response.set("Access-Control-Allow-Headers", allow_headers);
            response.set("Access-Control-Allow-Credentials", allow_credentials);
            response.prepare_payload();

            boost::beast::http::write(_socket, response, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            if (_socket.is_open()) _socket.close(ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif
        } catch (std::exception const& e) {
            #if defined(DEBUG) || defined(_DEBUG)
            EXCEPTION_ERROR(e);
            #endif
        }
    }
}