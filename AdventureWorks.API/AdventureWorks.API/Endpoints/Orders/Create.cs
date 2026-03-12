using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Orders.Create;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateOrderLineDto> OrderLines { get; set; } = new();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", async (
            Request request,
            ICommandHandler<CreateOrderCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateOrderCommand
            {
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                OrderLines = request.OrderLines
            };

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
