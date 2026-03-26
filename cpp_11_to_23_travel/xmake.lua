add_rules("mode.debug", "mode.release")
set_toolchains("llvm", { sdk = "/usr/local/opt/llvm" })
set_languages("c++23")
set_policy("build.c++.modules", true)

if is_plat("macosx") then
    add_cxflags("-mmacosx-version-min=15.0")
    add_ldflags("-mmacosx-version-min=15.0")
end

target("main")
    set_kind("binary")
    add_files("src/*.cpp", "src/*.mpp")
