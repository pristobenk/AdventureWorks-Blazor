using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
