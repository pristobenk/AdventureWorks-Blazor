using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Todos;

public sealed record TodoItemCreatedDomainEvent(Guid TodoItemId) : IDomainEvent;
