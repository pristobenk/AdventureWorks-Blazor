using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public sealed record OrderUpdatedDomainEvent(int OrderId) : IDomainEvent;
