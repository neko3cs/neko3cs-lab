add_rules("mode.debug", "mode.release")
add_requires("drogon", "gtest")

-- グローバル設定（すべてのターゲットに適用）
set_languages("c++20")
set_warnings("all", "extra")

if is_mode("release") then
    set_optimize("fastest")
else
    set_symbols("debug")
    set_optimize("none")
end

-- メインアプリ
target("hello_cpp_api")
    set_kind("binary")
    add_files("src/*.cc")
    add_packages("drogon")

-- テストプログラム
target("test")
    set_kind("binary")
    add_files("src/hello_api.cc", "src/todo_api.cc")
    add_files("test/*.cc")
    add_packages("drogon", "gtest")
