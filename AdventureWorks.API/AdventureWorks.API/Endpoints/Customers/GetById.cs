using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Customers.GetById;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Customers;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        app.MapGet("customers/{id:int}", async (
            int id,
            IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCustomerByIdQuery(id);

            var result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Customers)
        .RequireAuthorization();
    }
}
