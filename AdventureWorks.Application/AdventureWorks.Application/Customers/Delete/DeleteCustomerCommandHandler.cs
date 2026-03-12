using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Customers;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Customers.Delete;

internal sealed class DeleteCustomerCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteCustomerCommand>
{
    public async Task<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        Customer? customer = await context.Customers
            .SingleOrDefaultAsync(c => c.CustomerId == command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        context.Customers.Remove(customer);

        customer.Raise(new CustomerDeletedDomainEvent(customer.CustomerId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
