using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Products.Get;
using AdventureWorks.Application.Pagination;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.WebApi.Endpoints.Products;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
            IQueryHandler<GetProductsQuery, PagedList<GetProductsResponse>> handler,
            CancellationToken cancellationToken,
            [FromQuery] string? searchTerm,
            int page = 1,
            int pageSize = 10) =>
        {
            var query = new GetProductsQuery(searchTerm, page, pageSize);

            Result<PagedList<GetProductsResponse>> result = await handler.Handle(query, cancellationToken);
            
            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(new 
            {
                Items = result.Value.Items,
                TotalCount = result.Value.TotalCount
            });
        })
        .WithTags(Tags.Products)
        .RequireAuthorization();
    }
}

