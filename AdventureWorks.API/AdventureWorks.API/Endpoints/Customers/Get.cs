using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Customers.Get;
using AdventureWorks.Application.Pagination;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Customers;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        app.MapGet("customers", async (
            string? searchTerm,
            int page,
            int pageSize,
            IQueryHandler<GetCustomersQuery, PagedList<GetCustomersResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCustomersQuery(searchTerm, page == 0 ? 1 : page, pageSize == 0 ? 10 : pageSize);

            var result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Customers)
        .RequireAuthorization();
    }
}
