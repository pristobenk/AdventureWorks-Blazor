using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;
