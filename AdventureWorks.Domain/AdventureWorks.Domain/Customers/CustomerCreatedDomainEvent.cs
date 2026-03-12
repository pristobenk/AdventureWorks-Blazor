using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Customers;

public sealed record CustomerCreatedDomainEvent(int CustomerId) : IDomainEvent;
