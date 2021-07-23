cmake_minimum_required(VERSION 3.14)

# Set the project name to your project name, my project isn't very descriptive
project(Utility.Downsample CXX)
include(cmake/StandardProjectSettings.cmake)
include(cmake/PreventInSourceBuilds.cmake)

# Link this 'library' to set the c++ standard / compile-time options requested
add_library(project_options INTERFACE)
target_compile_features(project_options INTERFACE cxx_std_17)

if(CMAKE_CXX_COMPILER_ID MATCHES ".*Clang")
  option(ENABLE_BUILD_WITH_TIME_TRACE "Enable -ftime-trace to generate time tracing .json files on clang" OFF)
  if(ENABLE_BUILD_WITH_TIME_TRACE)
    target_compile_options(project_options INTERFACE -ftime-trace)
  endif()
endif()

# Link this 'library' to use the warnings specified in CompilerWarnings.cmake
add_library(project_warnings INTERFACE)

# standard compiler warnings
include(cmake/CompilerWarnings.cmake)
set_project_warnings(project_warnings)

# sanitizer options if supported by compiler
include(cmake/Sanitizers.cmake)
enable_sanitizers(project_options)

# enable doxygen
# include(cmake/Doxygen.cmake)
# enable_doxygen()

# allow for static analysis options
include(cmake/StaticAnalyzers.cmake)

option(ENABLE_TESTING "Enable Test Builds" OFF)

# Set up some extra Conan dependencies based on our needs before loading Conan
set(CONAN_EXTRA_REQUIRES "cgal/5.2.1")
set(CONAN_EXTRA_OPTIONS cgal:header_only=True cgal:with_cgal_core=True)

include(cmake/Conan.cmake)
run_conan()

# if(ENABLE_TESTING)
#   enable_testing()
#   message("Building Tests. Be sure to check out test/constexpr_tests for constexpr testing")
#   add_subdirectory(test)
# endif()

add_library(utility.downsample SHARED lib/downsample.cpp)
target_link_libraries(
  utility.downsample
 
  PRIVATE
    project_options
    CONAN_PKG::cgal
  )

install(
  TARGETS utility.downsample
  DESTINATION ${CMAKE_INSTALL_PREFIX}
)
