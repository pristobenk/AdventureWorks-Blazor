using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Orders;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Orders.Update;

internal sealed class UpdateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateOrderCommand>
{
    public async Task<Result> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        Order? order = await context.Orders
            .Include(o => o.OrderLines)
            .SingleOrDefaultAsync(o => o.OrderId == command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        order.CustomerId = command.CustomerId;
        order.OrderDate = DateTime.SpecifyKind(command.OrderDate, DateTimeKind.Utc);
        order.TotalAmount = command.TotalAmount;

        // naive synchronization of order lines: remove missing, update existing, add new
        var incomingIds = command.OrderLines.Where(l => l.OrderLineId.HasValue).Select(l => l.OrderLineId!.Value).ToHashSet();
        var toRemove = order.OrderLines.Where(l => !incomingIds.Contains(l.OrderLineId)).ToList();

        foreach (var r in toRemove) order.OrderLines.Remove(r);

        foreach (var l in command.OrderLines)
        {
            if (l.OrderLineId.HasValue)
            {
                var existing = order.OrderLines.SingleOrDefault(x => x.OrderLineId == l.OrderLineId.Value);
                if (existing is not null)
                {
                    existing.ProductId = l.ProductId;
                    existing.Quantity = l.Quantity;
                    existing.UnitPrice = l.UnitPrice;
                }
            }
            else
            {
                order.OrderLines.Add(new OrderLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                });
            }
        }

        order.Raise(new OrderUpdatedDomainEvent(order.OrderId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
