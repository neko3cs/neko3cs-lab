#ifndef SRC_TODO_API_H_
#define SRC_TODO_API_H_

#include <drogon/HttpController.h>

#include <atomic>
#include <map>
#include <mutex>
#include <string>

struct Todo
{
  int id;
  std::string title;
  bool completed;
};

class TodoApi : public drogon::HttpController<TodoApi>
{
public:
  METHOD_LIST_BEGIN
  ADD_METHOD_TO(TodoApi::GetTodos, "/todos", drogon::Get);
  ADD_METHOD_TO(TodoApi::GetOne, "/todos/{id}", drogon::Get);
  ADD_METHOD_TO(TodoApi::Create, "/todos", drogon::Post);
  ADD_METHOD_TO(TodoApi::Update, "/todos/{id}", drogon::Put);
  ADD_METHOD_TO(TodoApi::DeleteOne, "/todos/{id}", drogon::Delete);
  METHOD_LIST_END

  void GetTodos(const drogon::HttpRequestPtr &req,
                std::function<void(const drogon::HttpResponsePtr &)> &&callback);
  void GetOne(const drogon::HttpRequestPtr &req,
              std::function<void(const drogon::HttpResponsePtr &)> &&callback,
              int id);
  void Create(const drogon::HttpRequestPtr &req,
              std::function<void(const drogon::HttpResponsePtr &)> &&callback);
  void Update(const drogon::HttpRequestPtr &req,
              std::function<void(const drogon::HttpResponsePtr &)> &&callback,
              int id);
  void DeleteOne(const drogon::HttpRequestPtr &req,
                 std::function<void(const drogon::HttpResponsePtr &)> &&callback,
                 int id);

private:
  static std::map<int, Todo> todoStorage_;
  static std::atomic<int> nextId_;
  static std::mutex mutex_;
};

#endif // SRC_TODO_API_H_
