using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Products;

public sealed record ProductCreatedDomainEvent(Guid TodoItemId) : IDomainEvent;