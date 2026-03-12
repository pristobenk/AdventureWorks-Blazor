using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Abstractions.Authentication;
using AdventureWorks.Domain.Customers;
using AdventureWorks.SharedKernel;

namespace AdventureWorks.Application.Customers.Create;

internal sealed class CreateCustomerCommandHandler(IApplicationDbContext context, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateCustomerCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerName = command.CustomerName,
            ContactName = command.ContactName,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
            Address = command.Address
        };

        context.Customers.Add(customer);

        await context.SaveChangesAsync(cancellationToken);

        // raise domain event
        customer.Raise(new CustomerCreatedDomainEvent(customer.CustomerId));

        return Result.Success(customer.CustomerId);
    }
}
