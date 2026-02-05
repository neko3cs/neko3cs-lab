#include "todo_api.h"

std::map<int, Todo> TodoApi::todoStorage_;
std::atomic<int> TodoApi::nextId_{1};
std::mutex TodoApi::mutex_;

void TodoApi::GetTodos(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback)
{
  Json::Value ret(Json::arrayValue);
  std::lock_guard<std::mutex> lock(mutex_);
  for (const auto &pair : todoStorage_)
  {
    Json::Value item;
    item["id"] = pair.second.id;
    item["title"] = pair.second.title;
    item["completed"] = pair.second.completed;
    ret.append(item);
  }
  auto resp = drogon::HttpResponse::newHttpJsonResponse(ret);
  callback(resp);
}

void TodoApi::GetOne(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback, int id)
{
  std::lock_guard<std::mutex> lock(mutex_);
  auto it = todoStorage_.find(id);
  if (it != todoStorage_.end())
  {
    Json::Value ret;
    ret["id"] = it->second.id;
    ret["title"] = it->second.title;
    ret["completed"] = it->second.completed;
    callback(drogon::HttpResponse::newHttpJsonResponse(ret));
  }
  else
  {
    callback(drogon::HttpResponse::newHttpResponse(drogon::k404NotFound,
                                                   drogon::CT_APPLICATION_JSON));
  }
}

void TodoApi::Create(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback)
{
  auto json = req->getJsonObject();
  if (json && (*json)["title"].isString())
  {
    int id = nextId_++;
    Todo todo{id, (*json)["title"].asString(), (*json)["completed"].asBool()};
    {
      std::lock_guard<std::mutex> lock(mutex_);
      todoStorage_[id] = todo;
    }
    Json::Value ret;
    ret["id"] = todo.id;
    ret["title"] = todo.title;
    ret["completed"] = todo.completed;
    auto resp = drogon::HttpResponse::newHttpJsonResponse(ret);
    resp->setStatusCode(drogon::k201Created);
    callback(resp);
  }
  else
  {
    callback(drogon::HttpResponse::newHttpResponse(drogon::k400BadRequest,
                                                   drogon::CT_APPLICATION_JSON));
  }
}

void TodoApi::Update(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback, int id)
{
  auto json = req->getJsonObject();
  if (json)
  {
    std::lock_guard<std::mutex> lock(mutex_);
    auto it = todoStorage_.find(id);
    if (it != todoStorage_.end())
    {
      if ((*json)["title"].isString())
        it->second.title = (*json)["title"].asString();
      if ((*json)["completed"].isBool())
        it->second.completed = (*json)["completed"].asBool();

      Json::Value ret;
      ret["id"] = it->second.id;
      ret["title"] = it->second.title;
      ret["completed"] = it->second.completed;
      callback(drogon::HttpResponse::newHttpJsonResponse(ret));
    }
    else
    {
      callback(drogon::HttpResponse::newHttpResponse(
          drogon::k404NotFound, drogon::CT_APPLICATION_JSON));
    }
  }
  else
  {
    callback(drogon::HttpResponse::newHttpResponse(drogon::k400BadRequest,
                                                   drogon::CT_APPLICATION_JSON));
  }
}

void TodoApi::DeleteOne(
    const drogon::HttpRequestPtr &req,
    std::function<void(const drogon::HttpResponsePtr &)> &&callback, int id)
{
  std::lock_guard<std::mutex> lock(mutex_);
  if (todoStorage_.erase(id) > 0)
  {
    callback(drogon::HttpResponse::newHttpResponse(drogon::k204NoContent,
                                                   drogon::CT_APPLICATION_JSON));
  }
  else
  {
    callback(drogon::HttpResponse::newHttpResponse(drogon::k404NotFound,
                                                   drogon::CT_APPLICATION_JSON));
  }
}
