using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Products.Get;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Products;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
            IQueryHandler<GetProductsQuery, List<GetProductsResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetProductsQuery();

            Result<List<GetProductsResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Products)
        .RequireAuthorization();
    }
}

