#pragma once

#ifndef __API_GATEWAY_CONFIG_HPP__
#define __API_GATEWAY_CONFIG_HPP__

#include <safe_json.hpp>

#include <filesystem>

namespace gateway::config {
    void add_directory(std::filesystem::path _path);

    extern gateway::json::safe_object app;
}

#endif