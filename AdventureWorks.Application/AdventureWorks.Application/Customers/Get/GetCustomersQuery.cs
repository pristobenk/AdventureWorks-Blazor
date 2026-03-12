using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;

namespace AdventureWorks.Application.Customers.Get;

public sealed record GetCustomersQuery(string? SearchTerm, int Page = 1, int PageSize = 10) : IQuery<PagedList<GetCustomersResponse>>;
