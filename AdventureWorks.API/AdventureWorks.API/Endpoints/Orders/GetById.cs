using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Orders.GetById;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{orderId}", async (
            int orderId,
            IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetOrderByIdQuery(orderId);

            var result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
