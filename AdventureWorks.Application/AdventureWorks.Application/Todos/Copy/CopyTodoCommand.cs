using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Todos.Copy;

public sealed class CopyTodoCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public Guid TodoId { get; set; }
}
