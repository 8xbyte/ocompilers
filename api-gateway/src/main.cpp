#ifdef _WIN32
#include <sdkddkver.h>
#endif

#include <server.hpp>
#include <config.hpp>
#include <error.hpp>

int main(int argc, char** argv) {
    try {
        gateway::config::add_directory(argv[1]);
        gateway::server();
    } catch (std::exception const& e) {
        #if defined(DEBUG) || defined(_DEBUG)
        EXCEPTION_ERROR(e);
        #endif
    }
}