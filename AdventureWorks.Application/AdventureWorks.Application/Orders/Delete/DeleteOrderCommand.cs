using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Orders.Delete;

public sealed class DeleteOrderCommand : ICommand
{
    public int OrderId { get; set; }
}
