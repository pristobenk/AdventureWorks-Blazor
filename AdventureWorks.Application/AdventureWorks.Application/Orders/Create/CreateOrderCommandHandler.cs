using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Orders;
using AdventureWorks.SharedKernel;

namespace AdventureWorks.Application.Orders.Create;

internal sealed class CreateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, int>
{
    public async Task<Result<int>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerId = command.CustomerId,
            OrderDate = DateTime.SpecifyKind(command.OrderDate, DateTimeKind.Utc),
            TotalAmount = command.TotalAmount,
        };

        foreach (var l in command.OrderLines)
        {
            order.OrderLines.Add(new OrderLine
            {
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                UnitPrice = l.UnitPrice
            });
        }

        context.Orders.Add(order);

        await context.SaveChangesAsync(cancellationToken);

        order.Raise(new OrderCreatedDomainEvent(order.OrderId));

        return Result.Success(order.OrderId);
    }
}
