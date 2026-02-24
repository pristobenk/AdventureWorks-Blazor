using AdventureWorks.Domain.Users;

namespace AdventureWorks.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}
