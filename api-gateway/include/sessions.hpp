#pragma once

#ifndef __API_GATEWAY_SESSIONS_HPP__
#define __API_GATEWAY_SESSIONS_HPP__

#ifdef _WIN32
#include <sdkddkver.h>
#endif

#include <boost/asio.hpp>
#include <boost/beast.hpp>

namespace gateway {
    void base_session(boost::asio::ip::tcp::socket _socket);

    void http_session(
        boost::asio::ip::tcp::socket _socket, 
        boost::beast::http::request<boost::beast::http::string_body> _request, 
        std::string _to,
        bool _isAuth);

    void preflight_session(
        boost::asio::ip::tcp::socket _socket,
        boost::beast::http::request<boost::beast::http::string_body> _request);

    void websocket_session(
        boost::asio::ip::tcp::socket _socket, 
        boost::beast::http::request<boost::beast::http::string_body> _request, 
        std::string _to, 
        bool _isAuth);
}

#endif