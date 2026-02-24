using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
