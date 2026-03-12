using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public sealed record OrderDeletedDomainEvent(int OrderId) : IDomainEvent;
