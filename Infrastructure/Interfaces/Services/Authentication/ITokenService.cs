using Infrastructure.Identity.Models;

namespace Infrastructure.Interfaces.Services.Authentication;

public interface ITokenService
{
    string Token(User user);
}
