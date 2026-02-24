using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Todos;

public sealed record TodoItemCompletedDomainEvent(Guid TodoItemId) : IDomainEvent;
