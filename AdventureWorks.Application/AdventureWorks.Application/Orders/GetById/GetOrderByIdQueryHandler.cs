using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Domain.Orders;

namespace AdventureWorks.Application.Orders.GetById;

internal sealed class GetOrderByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        Order? order = await context.Orders
            .Include(o => o.OrderLines)
            .SingleOrDefaultAsync(o => o.OrderId == query.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<GetOrderByIdResponse>(OrderErrors.NotFound(query.OrderId));
        }

        var response = new GetOrderByIdResponse
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer != null ? order.Customer.CustomerName : string.Empty,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            OrderLines = order.OrderLines.Select(l => new GetOrderLineResponse
            {
                OrderLineId = l.OrderLineId,
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                UnitPrice = l.UnitPrice
            }).ToList()
        };

        return Result.Success(response);
    }
}
