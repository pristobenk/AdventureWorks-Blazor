using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Orders.Delete;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;


namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        app.MapDelete("orders/{orderId}", async (
            int orderId,
            ICommandHandler<DeleteOrderCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteOrderCommand { OrderId = orderId };

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent,  CustomResults.Problem);
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
