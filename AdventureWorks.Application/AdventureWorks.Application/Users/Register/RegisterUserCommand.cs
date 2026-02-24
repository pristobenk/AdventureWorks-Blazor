using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Users.Register;

public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password)
    : ICommand<Guid>;
