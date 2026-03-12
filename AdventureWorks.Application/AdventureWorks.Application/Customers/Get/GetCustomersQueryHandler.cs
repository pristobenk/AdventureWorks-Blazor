using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Customers.Get;

internal sealed class GetCustomersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetCustomersQuery, PagedList<GetCustomersResponse>>
{
    public async Task<Result<PagedList<GetCustomersResponse>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var customersQuery = context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            customersQuery = customersQuery.Where(c => c.CustomerName.Contains(query.SearchTerm));
        }

        var totalCount = await customersQuery.CountAsync(cancellationToken);

        List<GetCustomersResponse> customers = await customersQuery
            .OrderBy(c => c.CustomerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new GetCustomersResponse
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                ContactName = c.ContactName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address
            })
            .ToListAsync(cancellationToken);

        return new PagedList<GetCustomersResponse>(customers, totalCount, page, pageSize);
    }
}
