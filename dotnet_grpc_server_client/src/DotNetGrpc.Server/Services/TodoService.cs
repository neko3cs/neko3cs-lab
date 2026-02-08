using Grpc.Core;
using DotNetGrpc.Shared;

namespace DotNetGrpc.Server.Services;

public class TodoService : DotNetGrpc.Shared.Todo.TodoBase
{
    private static readonly List<TodoItem> _todos = new()
    {
        new TodoItem { Id = 1, Title = "Learn gRPC", IsCompleted = false },
        new TodoItem { Id = 2, Title = "Build a Blazor app", IsCompleted = false }
    };

    public override Task<TodoList> GetTodos(EmptyRequest request, ServerCallContext context)
    {
        var response = new TodoList();
        response.Items.AddRange(_todos);
        return Task.FromResult(response);
    }

    public override Task<TodoItem> AddTodo(AddTodoRequest request, ServerCallContext context)
    {
        var newItem = new TodoItem
        {
            Id = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1,
            Title = request.Title,
            IsCompleted = false
        };
        _todos.Add(newItem);
        return Task.FromResult(newItem);
    }

    public override Task<TodoItem> ToggleTodo(ToggleTodoRequest request, ServerCallContext context)
    {
        var item = _todos.FirstOrDefault(t => t.Id == request.Id);
        if (item != null)
        {
            item.IsCompleted = !item.IsCompleted;
        }
        return Task.FromResult(item ?? new TodoItem());
    }
}
