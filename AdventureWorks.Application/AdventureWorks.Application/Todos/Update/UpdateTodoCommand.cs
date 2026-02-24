using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid TodoItemId,
    string Description) : ICommand;
