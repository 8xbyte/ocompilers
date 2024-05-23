#include <sessions.hpp>
#include <check_auth.hpp>
#include <config.hpp>
#include <error.hpp>

#include <boost/lexical_cast.hpp>
#include <boost/algorithm/string.hpp>
#include <boost/url.hpp>

#include <iostream>
#include <format>
#include <string>
#include <thread>

namespace gateway {
    void websocket_session(
        boost::asio::ip::tcp::socket _socket,
        boost::beast::http::request<boost::beast::http::string_body> _request,
        std::string _to,
        bool _isAuth
    ) {
        try {
            boost::system::error_code ec;
            boost::asio::io_context context;

            auto to_url = boost::urls::url_view(_to);

            boost::beast::websocket::stream<boost::asio::ip::tcp::socket> input_websocket(std::move(_socket));

            int64_t user_id;
            if (_isAuth) {
                auto [is_success, user_id_buffer] = gateway::check_auth(std::move(_request));
                if (!is_success) {
                    input_websocket.close(boost::beast::websocket::close_code::policy_error, ec);
                    #if defined(DEBUG) || defined(_DEBUG)
                    IS_SYSTEM_ERROR(ec);
                    #endif

                    return;
                } else {
                    auto userIdFieldName = gateway::config::app.get_object("ApiGateway").get_object("Http").get_value("Headers").to_string();

                    input_websocket.set_option(
                        boost::beast::websocket::stream_base::decorator(
                            [&](boost::beast::http::response_header<>& hdr) {
                                hdr.set(userIdFieldName, std::to_string(user_id));
                            }));
                }
            }

            input_websocket.accept(_request, ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            boost::beast::websocket::stream<boost::asio::ip::tcp::socket> output_websocket(boost::asio::make_strand(context));
            output_websocket.next_layer().connect(boost::asio::ip::tcp::endpoint(
                boost::asio::ip::make_address(to_url.host()), boost::lexical_cast<uint16_t>(
                    to_url.port()
                )
            ), ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            output_websocket.handshake(to_url.host(), std::format("{}?{}", to_url.path(), to_url.query()), ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            auto websocket_message_handler = [&](
                boost::beast::websocket::stream<boost::asio::ip::tcp::socket>& _first_websocket,
                boost::beast::websocket::stream<boost::asio::ip::tcp::socket>& _second_websocket
                ) {
                    boost::beast::flat_buffer buffer;
                    try {
                        while (_first_websocket.is_open() && _second_websocket.is_open()) {
                            _first_websocket.read(buffer, ec);
                            #if defined(DEBUG) || defined(_DEBUG)
                            IS_SYSTEM_ERROR(ec);
                            #endif

                            _second_websocket.write(buffer.data(), ec);
                            #if defined(DEBUG) || defined(_DEBUG)
                            IS_SYSTEM_ERROR(ec);
                            #endif

                            buffer.clear();
                        }
                    } catch (std::exception const& e) {
                        #if defined(DEBUG) || defined(_DEBUG)
                        EXCEPTION_ERROR(e);
                        #endif
                    }
                };

            auto first_thread = std::thread(websocket_message_handler, std::ref(input_websocket), std::ref(output_websocket));
            auto second_thread = std::thread(websocket_message_handler, std::ref(output_websocket), std::ref(input_websocket));

            first_thread.join();
            second_thread.join();

            if (input_websocket.is_open()) input_websocket.close(output_websocket.reason(), ec);
            #if defined(DEBUG) || defined(_DEBUG)
            IS_SYSTEM_ERROR(ec);
            #endif

            if (output_websocket.is_open()) output_websocket.close(input_websocket.reason(), ec);
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