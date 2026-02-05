#include <drogon/drogon.h>
#include <iostream>

int main() {
    std::cout << "Server running on http://localhost:8080" << std::endl;
    drogon::app().addListener("0.0.0.0", 8080);
    drogon::app().run();
    return 0;
}
