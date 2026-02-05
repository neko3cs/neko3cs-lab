#include "hello_api.h"

void HelloApi::asyncHandleHttpRequest(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback)
{
  Json::Value ret;
  ret["message"] = "Hello from Drogon!";
  auto resp = drogon::HttpResponse::newHttpJsonResponse(ret);
  callback(resp);
}
