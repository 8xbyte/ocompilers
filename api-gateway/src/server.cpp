#include <server.hpp>
#include <sessions.hpp>
#include <error.hpp>
#include <config.hpp>

#include <boost/asio.hpp>
#include <boost/url.hpp>
#include <boost/lexical_cast.hpp>

#include <iostream>
#include <thread>

namespace gateway {
    void server() {
        auto server_url_string = gateway::config::app.get_object("ApiGateway").get_value("Url").to_string();
        auto server_url = boost::urls::parse_uri(server_url_string);

        boost::system::error_code ec;
        boost::asio::io_context context;
        boost::asio::ip::tcp::acceptor acceptor(context, boost::asio::ip::tcp::endpoint(
            boost::asio::ip::make_address(server_url.value().host()), boost::lexical_cast<uint16_t>(
                server_url.value().port()
            )
        ));


        while (1) {
            try {
                boost::asio::ip::tcp::socket socket(boost::asio::make_strand(context));
                acceptor.accept(socket, ec);
                #if defined(DEBUG) || defined(_DEBUG)
                IS_SYSTEM_ERROR(ec);
                #endif

                std::thread(gateway::base_session, std::move(socket)).detach();
            } catch (std::exception const& e) {
                #if defined(DEBUG) || defined(_DEBUG)
                EXCEPTION_ERROR(e);
                #endif
            }
        }
    }
}