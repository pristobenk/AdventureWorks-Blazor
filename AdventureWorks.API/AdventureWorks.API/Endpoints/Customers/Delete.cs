using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Customers.Delete;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Customers;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        app.MapDelete("customers/{id:int}", async (
            int id,
            ICommandHandler<DeleteCustomerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteCustomerCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Customers)
        .RequireAuthorization();
    }
}
