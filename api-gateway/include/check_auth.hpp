#pragma once

#ifndef __API_GATEWAY_CHECK_AUTH_HPP__
#define __API_GATEWAY_CHECK_AUTH_HPP__

#ifdef _WIN32
#include <sdkddkver.h>
#endif

#include <boost/beast.hpp>

namespace gateway {
    std::pair<bool, int64_t> check_auth(boost::beast::http::request<boost::beast::http::string_body> _request);
}

#endif