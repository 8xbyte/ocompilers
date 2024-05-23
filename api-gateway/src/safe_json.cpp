#include <safe_json.hpp>

namespace gateway::json {
    safe_value::safe_value() {}
    safe_value::safe_value(boost::json::value _value) {
        _Value = _value;
    }
    std::string safe_value::to_string() {
        std::unique_lock lock(_Mutex);
        return _Value.as_string().c_str();
    }
    uint64_t safe_value::to_uint64() {
        std::unique_lock lock(_Mutex);
        return _Value.as_uint64();
    }
    int64_t safe_value::to_int64() {
        std::unique_lock lock(_Mutex);
        return _Value.as_int64();
    }
    double safe_value::to_double() {
        std::unique_lock lock(_Mutex);
        return _Value.as_double();
    }
    bool safe_value::to_bool() {
        std::unique_lock lock(_Mutex);
        return _Value.as_bool();
    }

    safe_array::safe_array() {}
    safe_array::safe_array(boost::json::array _array) {
        _Array = _array;
    }
    gateway::json::safe_array safe_array::get_array(uint64_t _index) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_array(_Array[_index].as_array());
    }
    gateway::json::safe_object safe_array::get_object(uint64_t _index) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_object(_Array[_index].as_object());
    }
    gateway::json::safe_value safe_array::get_value(uint64_t _index) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_value(_Array[_index]);
    }
    size_t safe_array::size() {
        std::unique_lock lock(_Mutex);
        return _Array.size();
    }

    safe_object::safe_object() {}
    safe_object::safe_object(boost::json::object _object) {
        _Object = _object;
    }
    gateway::json::safe_array safe_object::get_array(std::string _key) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_array(_Object.at(_key).as_array());
    }
    gateway::json::safe_object safe_object::get_object(std::string _key) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_object(_Object.at(_key).as_object());
    }
    gateway::json::safe_value safe_object::get_value(std::string _key) {
        std::unique_lock lock(_Mutex);
        return gateway::json::safe_value(_Object.at(_key));
    }
    void safe_object::merge(boost::json::object _object) {
        for (auto field : _object) {
            std::unique_lock lock(_Mutex);
            _Object[field.key()] = field.value();
        }
    }
}