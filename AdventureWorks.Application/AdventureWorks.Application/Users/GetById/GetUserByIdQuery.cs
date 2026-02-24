using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
