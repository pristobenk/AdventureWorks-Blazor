using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;

namespace AdventureWorks.Application.Products.Get;


public sealed record GetProductsQuery(string? SearchTerm, int Page = 1, int PageSize = 10) : IQuery<PagedList<GetProductsResponse>>;

