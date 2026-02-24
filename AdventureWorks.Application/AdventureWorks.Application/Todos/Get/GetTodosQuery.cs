using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.Get;

public sealed record GetTodosQuery(Guid UserId) : IQuery<List<TodoResponse>>;
