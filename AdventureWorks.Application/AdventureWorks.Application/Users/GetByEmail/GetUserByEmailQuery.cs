using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
