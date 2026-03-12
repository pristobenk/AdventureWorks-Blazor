using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Orders;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Orders.Delete;

internal sealed class DeleteOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteOrderCommand>
{
    public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        Order? order = await context.Orders
            .Include(o => o.OrderLines)
            .SingleOrDefaultAsync(o => o.OrderId == command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        context.Orders.Remove(order);

        order.Raise(new OrderDeletedDomainEvent(order.OrderId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
