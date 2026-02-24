using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Todos;

public sealed record TodoItemDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;
