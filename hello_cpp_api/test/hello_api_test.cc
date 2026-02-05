#include <drogon/drogon.h>
#include <gtest/gtest.h>

#include <chrono>
#include <thread>

class HelloApiTest : public ::testing::Test {
 protected:
};

TEST_F(HelloApiTest, GetHello) {
  auto client = drogon::HttpClient::newHttpClient("http://127.0.0.1:8888");
  auto req = drogon::HttpRequest::newHttpRequest();
  req->setPath("/hello");
  req->setMethod(drogon::Get);

  bool finished = false;
  client->sendRequest(req, [&](drogon::ReqResult result,
                               const drogon::HttpResponsePtr& resp) {
    ASSERT_EQ(result, drogon::ReqResult::Ok);
    ASSERT_EQ(resp->getStatusCode(), drogon::k200OK);
    ASSERT_EQ((*resp->getJsonObject())["message"].asString(),
              "Hello from Drogon!");
    finished = true;
  });

  // Wait for async response
  for (int i = 0; i < 100 && !finished; ++i) {
    std::this_thread::sleep_for(std::chrono::milliseconds(10));
  }
  EXPECT_TRUE(finished);
}
