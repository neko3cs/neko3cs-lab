// #include <expected>
// #include <optional>
// #include <print>
// #include <ranges>
// #include <string>
// #include <string_view>
// #include <vector>
import std;
import hello;

#ifdef _WIN32
#define PLATFORM "Windows"
#elifdef __linux__
#define PLATFORM "Linux"
#elifdef __APPLE__
#define PLATFORM "macOS"
#else
#warning "不明なプラットフォーム"
#endif

void chapter1() {
  std::println("Hello, {}!", "World");
  std::print("x = {}, y = {}", 42, 3.14);
  std::println();
}

void chapter2() {
  std::vector v = {1, 2, 3, 4, 5, 6};

  for (auto x : v | std::views::filter([](int n) { return n % 2 == 0; }) |
                    std::views::transform([](int n) { return n * n; })) {
    std::print("{}, ", x);
  }
  std::println();
}

std::optional<int> parse(std::string_view s) {
  if (s.empty())
    return std::nullopt;
  return std::stoi(std::string(s));
}

void chapter3() {
  auto result = parse("42")
                    .transform([](int x) { return x * 2; })
                    .and_then([](int x) -> std::optional<int> {
                      if (x > 50)
                        return x;
                      return std::nullopt;
                    });

  std::println("{}", result.value_or(0));
}

std::expected<int, std::string> divide(int a, int b) {
  if (b == 0)
    return std::unexpected("ゼロ除算");
  return a / b;
}

void chapter4() {
  auto r = divide(10, 2);
  if (r)
    std::println("結果: {}", *r);

  auto err = divide(10, 0);
  if (!err)
    std::println("エラー: {}", err.error());

  auto chained = divide(10, 2).transform([](int x) { return x + 1; });
  if (chained)
    std::println("結果: {}", *chained);
}

struct Builder {
  int value = 0;

  auto &setValue(this auto &self, int v) {
    self.value = v;
    return self;
  }
};

struct Matrix {
  float data[4][4];
  float &operator[](std::size_t i, std::size_t j) { return data[i][j]; }
};

void chapter8() {
  Matrix m;
  m[1, 2] = 3.14f;
  std::println("{}", m[1, 2]);
}

void chapter9() { std::println("Platform: {}", PLATFORM); }

void chapter10() { hello(); }

int main(int argc, char **argv) {
  std::println("=== chapter1 ===");
  chapter1();
  std::println("=== chapter2 ===");
  chapter2();
  std::println("=== chapter3 ===");
  chapter3();
  std::println("=== chapter4 ===");
  chapter4();
  std::println("=== chapter8 ===");
  chapter8();
  std::println("=== chapter9 ===");
  chapter9();
  std::println("=== chapter10 ===");
  chapter10();

  return 0;
}
