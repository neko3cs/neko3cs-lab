#include "HelloApi.h"

void HelloApi::asyncHandleHttpRequest(const HttpRequestPtr& req, std::function<void (const HttpResponsePtr &)> &&callback)
{
    Json::Value ret;
    ret["message"] = "Hello from Drogon!";
    auto resp = HttpResponse::newHttpJsonResponse(ret);
    callback(resp);
}
