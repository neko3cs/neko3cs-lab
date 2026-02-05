#include <gtest/gtest.h>
#include <drogon/drogon.h>
#include <thread>
#include <chrono>

using namespace drogon;

class TodoApiTest : public ::testing::Test {
protected:
    static void SetUpTestSuite() {
        std::thread([]() {
            drogon::app().addListener("127.0.0.1", 8889);
            drogon::app().run();
        }).detach();
        std::this_thread::sleep_for(std::chrono::milliseconds(500));
    }

    static void TearDownTestSuite() {
        drogon::app().quit();
    }
    
    HttpResponsePtr sendRequest(const std::string &path, HttpMethod method, const Json::Value &body = Json::nullValue) {
        auto client = HttpClient::newHttpClient("http://127.0.0.1:8889");
        auto req = HttpRequest::newHttpRequest();
        req->setPath(path);
        req->setMethod(method);
        if (!body.isNull()) {
            Json::StreamWriterBuilder builder;
            req->setBody(Json::writeString(builder, body));
            req->setContentTypeCode(CT_APPLICATION_JSON);
        }
        
        HttpResponsePtr response;
        bool finished = false;
        client->sendRequest(req, [&](ReqResult result, const HttpResponsePtr &resp) {
            if (result == ReqResult::Ok) {
                response = resp;
            }
            finished = true;
        });
        
        for (int i = 0; i < 200 && !finished; ++i) {
            std::this_thread::sleep_for(std::chrono::milliseconds(10));
        }
        return response;
    }
};

TEST_F(TodoApiTest, FullCrudScenario) {
    // 1. 3件データをPOSTする
    Json::Value todo1; todo1["title"] = "Todo 1"; todo1["completed"] = false;
    Json::Value todo2; todo2["title"] = "Todo 2"; todo2["completed"] = true;
    Json::Value todo3; todo3["title"] = "Todo 3"; todo3["completed"] = false;
    
    auto resp1 = sendRequest("/todos", Post, todo1);
    ASSERT_NE(resp1, nullptr);
    ASSERT_EQ(resp1->getStatusCode(), k201Created);
    int id1 = (*resp1->getJsonObject())["id"].asInt();
    
    auto resp2 = sendRequest("/todos", Post, todo2);
    ASSERT_NE(resp2, nullptr);
    ASSERT_EQ(resp2->getStatusCode(), k201Created);
    int id2 = (*resp2->getJsonObject())["id"].asInt();

    auto resp3 = sendRequest("/todos", Post, todo3);
    ASSERT_NE(resp3, nullptr);
    ASSERT_EQ(resp3->getStatusCode(), k201Created);
    int id3 = (*resp3->getJsonObject())["id"].asInt();

    // 2. 全件データをGETする -> 3件取得されること, 内容が1.のデータとあってること
    auto respList = sendRequest("/todos", Get);
    ASSERT_NE(respList, nullptr);
    ASSERT_EQ(respList->getStatusCode(), k200OK);
    auto list = respList->getJsonObject();
    ASSERT_TRUE(list->isArray());
    ASSERT_EQ(list->size(), 3);
    
    // Check if ids match (order might not be guaranteed but storage is std::map)
    bool found1 = false, found2 = false, found3 = false;
    for (const auto& item : *list) {
        if (item["id"].asInt() == id1) {
            EXPECT_EQ(item["title"].asString(), "Todo 1");
            found1 = true;
        } else if (item["id"].asInt() == id2) {
            EXPECT_EQ(item["title"].asString(), "Todo 2");
            found2 = true;
        } else if (item["id"].asInt() == id3) {
            EXPECT_EQ(item["title"].asString(), "Todo 3");
            found3 = true;
        }
    }
    EXPECT_TRUE(found1); EXPECT_TRUE(found2); EXPECT_TRUE(found3);

    // 3. 1件データをPUTする
    Json::Value updateData;
    updateData["title"] = "Updated Todo 2";
    updateData["completed"] = false;
    auto respUpdate = sendRequest("/todos/" + std::to_string(id2), Put, updateData);
    ASSERT_NE(respUpdate, nullptr);
    ASSERT_EQ(respUpdate->getStatusCode(), k200OK);

    // 4. PUTした1件を取得する -> 1件取得されること, 3.の内容に更新されていること
    auto respGetOne = sendRequest("/todos/" + std::to_string(id2), Get);
    ASSERT_NE(respGetOne, nullptr);
    ASSERT_EQ(respGetOne->getStatusCode(), k200OK);
    auto updatedItem = respGetOne->getJsonObject();
    EXPECT_EQ((*updatedItem)["title"].asString(), "Updated Todo 2");
    EXPECT_EQ((*updatedItem)["completed"].asBool(), false);

    // 5. 1件DELETEする
    auto respDelete = sendRequest("/todos/" + std::to_string(id1), Delete);
    ASSERT_NE(respDelete, nullptr);
    ASSERT_EQ(respDelete->getStatusCode(), k204NoContent);

    // 6. 全件GETする -> 5.で削除したデータがないこと
    auto respFinalList = sendRequest("/todos", Get);
    ASSERT_NE(respFinalList, nullptr);
    ASSERT_EQ(respFinalList->getStatusCode(), k200OK);
    auto finalList = respFinalList->getJsonObject();
    ASSERT_EQ(finalList->size(), 2);
    
    for (const auto& item : *finalList) {
        EXPECT_NE(item["id"].asInt(), id1);
    }
}
