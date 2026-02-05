#include <drogon/drogon.h>
#include <gtest/gtest.h>
#include <thread>

class DrogonEnvironment : public ::testing::Environment
{
public:
  void SetUp() override
  {
    std::thread([]()
                {
      // 全てのテストで共通のポート 8888 を使用
      drogon::app().addListener("127.0.0.1", 8888);
      drogon::app().run(); })
        .detach();
    // サーバーの起動を待機
    std::this_thread::sleep_for(std::chrono::milliseconds(500));
  }

  void TearDown() override
  {
    drogon::app().quit();
  }
};

int main(int argc, char **argv)
{
  testing::InitGoogleTest(&argc, argv);
  testing::AddGlobalTestEnvironment(new DrogonEnvironment);
  return RUN_ALL_TESTS();
}
