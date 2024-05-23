#pragma once

#ifndef __API_GATEWAY_ERROR_HPP__
#define __API_GATEWAY_ERROR_HPP__

#include <iostream>
#include <format>

#define IS_SYSTEM_ERROR(ec) if (ec) std::cerr << std::format("[{}][{}]->{}", __FILE__, __LINE__, ec.what()) << std::endl
#define EXCEPTION_ERROR(e) std::cerr << std::format("[{}][{}]->{}", __FILE__, __LINE__, e.what()) << std::endl

#endif