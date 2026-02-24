using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
