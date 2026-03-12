using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Customers;

public sealed record CustomerDeletedDomainEvent(int CustomerId) : IDomainEvent;
