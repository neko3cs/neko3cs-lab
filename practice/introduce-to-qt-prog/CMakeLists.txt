cmake_minimum_required(VERSION 3.16)

project(IntroduceToQtProg VERSION 1.0.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(Qt6 REQUIRED COMPONENTS
  Widgets
)
qt_standard_project_setup()

qt_add_executable(IntroduceToQtProg
    main.cpp
)

target_link_libraries(IntroduceToQtProg
  PRIVATE
    Qt6::Widgets
)

set_target_properties(IntroduceToQtProg
  PROPERTIES
    WIN32_EXECUTABLE ON
    MACOSX_BUNDLE ON
)
