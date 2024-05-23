#pragma once

#ifndef __API_GATEWAY_SAFE_JSON_HPP__
#define __API_GATEWAY_SAFE_JSON_HPP__

#ifdef _WIN32
#include <sdkddkver.h>
#endif

#include <boost/json.hpp>

#include <string>
#include <mutex>

namespace gateway::json {
    class safe_value;
    class safe_object;
    class safe_array;

    class safe_value {
    public:
        explicit safe_value();
        explicit safe_value(boost::json::value _value);

        std::string to_string();
        uint64_t to_uint64();
        int64_t to_int64();
        double to_double();
        bool to_bool();

    private:
        std::mutex _Mutex;
        boost::json::value _Value;
    };

    class safe_array {
    public:
        explicit safe_array();
        explicit safe_array(boost::json::array _array);

        gateway::json::safe_array get_array(uint64_t _index);
        gateway::json::safe_object get_object(uint64_t _index);
        gateway::json::safe_value get_value(uint64_t _index);

        size_t size();

    private:
        std::mutex _Mutex;
        boost::json::array _Array;
    };

    class safe_object {
    public:
        explicit safe_object();
        explicit safe_object(boost::json::object _object);

        gateway::json::safe_array get_array(std::string _key);
        gateway::json::safe_object get_object(std::string _key);
        gateway::json::safe_value get_value(std::string _key);

        void merge(boost::json::object _object);

    private:
        std::mutex _Mutex;
        boost::json::object _Object;
    };
}

#endif