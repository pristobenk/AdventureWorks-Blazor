using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Products.Get;

public sealed record GetProductsQuery() : IQuery<List<GetProductsResponse>>;

