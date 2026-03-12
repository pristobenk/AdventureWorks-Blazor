using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Customers;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Customers.GetById;

internal sealed class GetCustomerByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
{
    public async Task<Result<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        Customer? customer = await context.Customers
            .SingleOrDefaultAsync(c => c.CustomerId == query.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure<GetCustomerByIdResponse>(CustomerErrors.NotFound(query.CustomerId));
        }

        var response = new GetCustomerByIdResponse
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            ContactName = customer.ContactName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address
        };

        return Result.Success(response);
    }
}
