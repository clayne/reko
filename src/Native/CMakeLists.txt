cmake_minimum_required(VERSION 3.10 FATAL_ERROR)

project(reko-native)

include(${CMAKE_CURRENT_LIST_DIR}/reko_platforms.cmake)

set(CMAKE_CXX_STANDARD 14)
set(CMAKE_C_STANDARD 11)

# No library prefix for consistent naming (Windows has no lib prefix)
# set(CMAKE_SHARED_LIBRARY_PREFIX "")

if(NOT REKO_ROOT)
	get_filename_component(
		REKO_ROOT "../../"
		REALPATH
		BASE_DIR "${CMAKE_SOURCE_DIR}"
	)
	message(STATUS "Reko directory: ${REKO_ROOT}")
endif()

include (CheckSymbolExists)
configure_file(${CMAKE_SOURCE_DIR}/config.h.in ${CMAKE_BINARY_DIR}/config.h)

include_directories(
	${CMAKE_BINARY_DIR}
	${CMAKE_SOURCE_DIR}
	${CMAKE_SOURCE_DIR}/include
)

set(REKO_BUNDLED_LIBDIRS "")

if(MSVC)
	if(REKO_PLATFORM_X64)
		list(APPEND REKO_BUNDLED_LIBDIRS "${REKO_ROOT}/external/Capstone/X64")
	else()
		list(APPEND REKO_BUNDLED_LIBDIRS "${REKO_ROOT}/external/Capstone/X86")
	endif()
endif()

if((UNIX OR MINGW) AND NOT MSVC)
	find_package(PkgConfig REQUIRED)
	pkg_check_modules(CAPSTONE REQUIRED capstone)

	link_directories(
		${CAPSTONE_LIBRARY_DIRS}
	)

	include_directories(
		${CAPSTONE_INCLUDE_DIRS}
	)
else()
	find_library(CAPSTONE_LIBRARIES capstone PATHS ${REKO_BUNDLED_LIBDIRS} REQUIRED)
endif()

if(MSVC)
	include(ProcessorCount)
	ProcessorCount(NUM_THREADS)
	add_definitions("/MP${NUM_THREADS}")
endif()

add_subdirectory(common)
add_subdirectory(ArmNative)

