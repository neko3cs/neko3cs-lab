#include <gtest/gtest.h>
#include <drogon/drogon.h>
#include <thread>
#include <chrono>

class HelloApiTest : public ::testing::Test {
protected:
    static void SetUpTestSuite() {
        // Run drogon in a separate thread
        std::thread([]() {
            drogon::app().addListener("127.0.0.1", 8888);
            drogon::app().run();
        }).detach();
        
        // Wait for server to start
        std::this_thread::sleep_for(std::chrono::milliseconds(500));
    }

    static void TearDownTestSuite() {
        drogon::app().quit();
    }
};

TEST_F(HelloApiTest, GetHello) {
    auto client = drogon::HttpClient::newHttpClient("http://127.0.0.1:8888");
    auto req = drogon::HttpRequest::newHttpRequest();
    req->setPath("/hello");
    req->setMethod(drogon::Get);

    bool finished = false;
    client->sendRequest(req, [&](drogon::ReqResult result, const drogon::HttpResponsePtr &resp) {
        ASSERT_EQ(result, drogon::ReqResult::Ok);
        ASSERT_EQ(resp->getStatusCode(), drogon::k200OK);
        ASSERT_EQ((*resp->getJsonObject())["message"].asString(), "Hello from Drogon!");
        finished = true;
    });

    // Wait for async response
    for (int i = 0; i < 100 && !finished; ++i) {
        std::this_thread::sleep_for(std::chrono::milliseconds(10));
    }
    EXPECT_TRUE(finished);
}

int main(int argc, char **argv) {
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}
