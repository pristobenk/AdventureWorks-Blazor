using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Abstractions.Authentication;
using AdventureWorks.Domain.Customers;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Customers.Update;

internal sealed class UpdateCustomerCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        Customer? customer = await context.Customers
            .SingleOrDefaultAsync(c => c.CustomerId == command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        customer.CustomerName = command.CustomerName;
        customer.ContactName = command.ContactName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.Email = command.Email;
        customer.Address = command.Address;

        customer.Raise(new CustomerUpdatedDomainEvent(customer.CustomerId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
