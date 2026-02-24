using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Products;

public sealed record ProductUpdatedDomainEvent(Guid TodoItemId) : IDomainEvent;