using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Orders.Update;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<UpdateOrderLineDto> OrderLines { get; set; } = new();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("orders/{orderId}", async (
            int orderId,
            Request request,
            ICommandHandler<UpdateOrderCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateOrderCommand
            {
                OrderId = orderId,
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                OrderLines = request.OrderLines
            };

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
