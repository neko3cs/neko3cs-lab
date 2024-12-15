cmake_minimum_required(VERSION 3.16)
project(OpenGLCpp)

set(CMAKE_CXX_FLAGS "-std=c++20")
add_compile_options(-Wall -O3)

# deprecatedな宣言の警告無効化（macOSではOpenGLが非推奨になっているため）
add_definitions(-Wno-deprecated-declarations)

# OpenGLライブラリの追加
find_package(OpenGL REQUIRED)
find_package(GLUT REQUIRED)
set(INCLUDE_DIRS ${OPENGL_INCLUDE_DIR} ${GLUT_INCLUDE_DIR})
set(LIBRARIES ${OPENGL_LIBRARIES} ${GLUT_LIBRARIES})
include_directories(${INCLUDE_DIRS})

add_executable(
    a.out
    main.cc
)

target_link_libraries(
    a.out
    ${LIBRARIES}
)
