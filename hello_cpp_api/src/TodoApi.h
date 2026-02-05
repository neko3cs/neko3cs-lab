#pragma once

#include <drogon/HttpController.h>
#include <map>
#include <mutex>
#include <atomic>
#include <string>

using namespace drogon;

struct Todo {
    int id;
    std::string title;
    bool completed;
};

class TodoApi : public drogon::HttpController<TodoApi>
{
  public:
    METHOD_LIST_BEGIN
    ADD_METHOD_TO(TodoApi::getTodos, "/todos", Get);
    ADD_METHOD_TO(TodoApi::getOne, "/todos/{id}", Get);
    ADD_METHOD_TO(TodoApi::create, "/todos", Post);
    ADD_METHOD_TO(TodoApi::update, "/todos/{id}", Put);
    ADD_METHOD_TO(TodoApi::deleteOne, "/todos/{id}", Delete);
    METHOD_LIST_END

    void getTodos(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback);
    void getOne(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback, int id);
    void create(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback);
    void update(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback, int id);
    void deleteOne(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback, int id);

  private:
    static std::map<int, Todo> todoStorage_;
    static std::atomic<int> nextId_;
    static std::mutex mutex_;
};
