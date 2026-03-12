using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Customers.Create;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Customers;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public void MapEndpoint(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        app.MapPost("customers", async (
            Request request,
            ICommandHandler<CreateCustomerCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCustomerCommand
            {
                CustomerName = request.CustomerName,
                ContactName = request.ContactName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Address
            };

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Customers)
        .RequireAuthorization();
    }
}
