cmake_minimum_required(VERSION 3.16)
project(QtQuickSampleApp)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
add_compile_options(-Wall -O3)

find_package(Qt6 REQUIRED COMPONENTS
  Core
  Quick
)

# Qt関係の設定を自動でオンにする
# これを設定しておけば、特にQt6で導入されたCMakeヘルパーマクロを使う必要はない
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC ON)

# qrcファイルにてqmlファイルの紐付けを行う
add_executable(${PROJECT_NAME}
  src/main.cpp
  src/qml.qrc
)

target_link_libraries(${PROJECT_NAME}
    PRIVATE
        Qt6::Core
        Qt6::Quick
)
