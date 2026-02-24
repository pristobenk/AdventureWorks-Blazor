using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Products;

public sealed record ProductDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;