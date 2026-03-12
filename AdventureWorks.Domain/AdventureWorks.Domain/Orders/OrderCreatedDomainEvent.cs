using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public sealed record OrderCreatedDomainEvent(int OrderId) : IDomainEvent;
