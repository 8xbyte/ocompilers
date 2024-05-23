#include <check_auth.hpp>
#include <config.hpp>
#include <error.hpp>

#include <boost/algorithm/string.hpp>
#include <boost/asio.hpp>
#include <boost/url.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/json.hpp>

#include <iostream>
#include <format>

namespace gateway {
    std::pair<bool, int64_t> check_auth(boost::beast::http::request<boost::beast::http::string_body> _request) {
        try {
            auto url_string = gateway::config::app.get_object("Auth").get_value("Url").to_string();
            auto url = boost::urls::parse_uri(url_string);
            if (url.has_error()) std::cerr << url.error() << std::endl;

            auto auth_path = gateway::config::app.get_object("ApiGateway").get_value("VerifyPath").to_string();
            auto auth_token_name = gateway::config::app.get_object("ApiGateway").get_object("Http").get_object("Cookies").get_value("AuthToken").to_string();
            std::string token_string;

            auto cookies = _request.find(boost::beast::http::field::cookie);
            if (cookies != _request.end()) {
                std::vector<std::string> split_cookies;
                boost::split(split_cookies, cookies->value(), boost::is_any_of("; "));
                for (auto cookie : split_cookies) {
                    std::vector<std::string> key_value;
                    boost::split(key_value, cookie, boost::is_any_of("="));
                    if (key_value.size() == 2 && key_value[0] == auth_token_name) {
                        token_string = key_value[1];
                    }
                }
            }

            boost::system::error_code ec;
            boost::asio::io_context context;
            boost::asio::ip::tcp::socket socket(context);
            socket.connect(boost::asio::ip::tcp::endpoint(
                boost::asio::ip::make_address(url.value().host()), boost::lexical_cast<uint16_t>(
                    url.value().port()
                )
            ), ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            boost::beast::http::request<boost::beast::http::string_body> request(boost::beast::http::verb::post, auth_path, 11);
            request.set(boost::beast::http::field::cookie, std::format("{}={}", auth_token_name, token_string));
            request.set(boost::beast::http::field::host, _request[boost::beast::http::field::host]);
            boost::beast::http::write(socket, request, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            boost::beast::flat_buffer buffer;
            boost::beast::http::response<boost::beast::http::string_body> response;
            boost::beast::http::read(socket, buffer, response, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            if (response.result() == boost::beast::http::status::ok) {
                auto userIdFieldName = gateway::config::app.get_object("Auth").get_object("Jwt")
                    .get_object("Token").get_object("Fields").get_value("UserId").to_string();

                auto userId = boost::json::parse(response.body()).as_object()[userIdFieldName].as_int64();

                return { true, userId };
            } else {
                return { false, 0 };
            }
        } catch (std::exception const& e) {
            #if defined(DEBUG) || defined(_DEBUG)
            EXCEPTION_ERROR(e);
            #endif
            return { false, 0 };
        }
    }
}