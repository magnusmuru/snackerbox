using System.IdentityModel.Tokens.Jwt;

namespace SnackerBox.Services.Interfaces;

public interface IAuthenticationService
{
    JwtSecurityToken AuthenticateUser(string userName);
    bool ComparePassword(string password, string passwordHash);
}