using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
