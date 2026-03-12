using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Customers;

public sealed record CustomerUpdatedDomainEvent(int CustomerId) : IDomainEvent;
