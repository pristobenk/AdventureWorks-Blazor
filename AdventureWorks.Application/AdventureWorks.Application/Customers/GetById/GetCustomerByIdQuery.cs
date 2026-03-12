using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Customers.GetById;

public sealed record GetCustomerByIdQuery(int CustomerId) : IQuery<GetCustomerByIdResponse>;
