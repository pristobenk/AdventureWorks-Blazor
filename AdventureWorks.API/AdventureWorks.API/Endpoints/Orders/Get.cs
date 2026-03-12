using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Orders.Get;
using AdventureWorks.Application.Pagination;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (
            [FromQuery] string? searchTerm,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IQueryHandler<GetOrdersQuery, PagedList<GetOrdersResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetOrdersQuery(searchTerm, page == 0 ? 1 : page, pageSize == 0 ? 10 : pageSize);

            var result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
