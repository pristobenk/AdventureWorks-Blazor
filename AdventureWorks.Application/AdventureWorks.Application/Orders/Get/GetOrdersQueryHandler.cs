using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;
using AdventureWorks.Domain.Orders;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Orders.Get;

internal sealed class GetOrdersQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, PagedList<GetOrdersResponse>>
{
    public async Task<Result<PagedList<GetOrdersResponse>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        IQueryable<Order> ordersQuery = context.Orders.AsQueryable().Include(o => o.OrderLines);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            ordersQuery = ordersQuery.Where(o => o.OrderId.ToString().Contains(query.SearchTerm)
                || o.CustomerId.ToString().Contains(query.SearchTerm)
                || (o.Customer != null && o.Customer.CustomerName.Contains(query.SearchTerm)));
        }

        var totalCount = await ordersQuery.CountAsync(cancellationToken);

        var orders = await ordersQuery
            .OrderBy(o => o.OrderId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new GetOrdersResponse
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer != null ? o.Customer.CustomerName : string.Empty,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                OrderLines = o.OrderLines.Select(l => new GetOrderLineResponse
                {
                    OrderLineId = l.OrderLineId,
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new PagedList<GetOrdersResponse>(orders, totalCount, page, pageSize);
    }
}
