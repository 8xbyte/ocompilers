#include <config.hpp>

#include <boost/json.hpp>

#include <iostream>
#include <fstream>

namespace gateway::config {
    void add_directory(std::filesystem::path _path) {
        std::cout << "Add config directory: " << _path.string() << std::endl;
        for (auto file : std::filesystem::directory_iterator(_path)) {
            if (file.path().extension() == ".json") {
                std::cout << "Load " << file.path().string() << std::endl;
                std::fstream file_stream(file.path(), std::ios::in);
                if (!file_stream.is_open()) {
                    std::cout << "Failed load " << file.path().string() << std::endl;
                    continue;
                }
                auto json_object = boost::json::parse(file_stream);
                gateway::config::app.merge(json_object.as_object());
            }
        }
    }

    gateway::json::safe_object app;
}