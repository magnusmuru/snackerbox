using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SnackerBox.Configs;
using SnackerBox.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace SnackerBox.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly SnackerConfig _config;
    private readonly ILogger _logger;

    public AuthenticationService(SnackerConfig config,
        ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public JwtSecurityToken AuthenticateUser(string userName)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, "API User")
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtConfig.Secret));

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _config.JwtConfig.ValidIssuer,
            audience: _config.JwtConfig.ValidAudience,
            expires: DateTime.Now.AddMinutes(_config.JwtConfig.ExpirationMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        _logger.Debug("User: {User} will be issued a new JWT", userName);

        return jwtSecurityToken;
    }

    public bool ComparePassword(string password, string passwordHash)
    {
        // Bypassing BCrypt to have localized implementation
        // Actual application would be, for obvious reasons, hashed and salted
        // return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        return password == passwordHash;
    }
}