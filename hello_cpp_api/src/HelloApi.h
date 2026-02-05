#pragma once

#include <drogon/HttpSimpleController.h>

using namespace drogon;

class HelloApi : public drogon::HttpSimpleController<HelloApi>
{
  public:
    void asyncHandleHttpRequest(const HttpRequestPtr& req, std::function<void (const HttpResponsePtr &)> &&callback) override;
    PATH_LIST_BEGIN
    PATH_ADD("/hello", Get);
    PATH_LIST_END
};
