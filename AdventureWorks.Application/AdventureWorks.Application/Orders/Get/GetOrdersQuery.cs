using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;

namespace AdventureWorks.Application.Orders.Get;

public sealed record GetOrdersQuery(string? SearchTerm, int Page = 1, int PageSize = 10) : IQuery<PagedList<GetOrdersResponse>>;
