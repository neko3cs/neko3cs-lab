cmake_minimum_required(VERSION 3.16)
project(QtQuickBasicApp)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
add_compile_options(-Wall -O3)

find_package(Qt6 REQUIRED COMPONENTS
  Quick
  Gui
)
qt_standard_project_setup()

qt_add_executable(QtQuickBasicApp
  src/main.cpp
)

qt_add_qml_module(QtQuickBasicApp
  URI hello
  QML_FILES
    src/mainwindow.qml
)

target_link_libraries(QtQuickBasicApp
  PRIVATE
    Qt6::Quick
    Qt6::Gui
)
