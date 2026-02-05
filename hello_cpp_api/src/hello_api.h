#ifndef SRC_HELLO_API_H_
#define SRC_HELLO_API_H_

#include <drogon/HttpSimpleController.h>

class HelloApi : public drogon::HttpSimpleController<HelloApi>
{
public:
    void asyncHandleHttpRequest(
        const drogon::HttpRequestPtr &req,
        std::function<void(const drogon::HttpResponsePtr &)> &&callback) override;
    PATH_LIST_BEGIN
    PATH_ADD("/hello", drogon::Get);
    PATH_LIST_END
};

#endif // SRC_HELLO_API_H_
