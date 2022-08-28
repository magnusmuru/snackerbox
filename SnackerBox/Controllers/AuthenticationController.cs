using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using SnackerBox.Dto;
using SnackerBox.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace SnackerBox.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;

    public AuthenticationController(IAuthenticationService authenticationService,
        IUserService userService,
        ILogger logger)
    {
        _authenticationService = authenticationService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    public Task<ActionResult> AuthenticateUser([FromForm] AuthenticationDto authenticationDto)
    {
        var user = _userService.FindUserByUsername(authenticationDto.Username);
        if (user == null)
        {
            _logger.Error("Unknown User {Username} is attempting to query Authentication API",
                authenticationDto.Username);
            return Task.FromResult<ActionResult>(Unauthorized(
                "You are not a user of Snacker. Contact System Administrators if you believe this is a fault"));
        }

        if (!_authenticationService.ComparePassword(authenticationDto.Password, user.Password))
        {
            _logger.Error("User: {User} with is not authorized to use Snacker",
                user.Username);
            return Task.FromResult<ActionResult>(Unauthorized(
                "This user and password is incorrect. Contact System Administrators if you believe this is a fault"));
        }

        var jwtSecurityToken = _authenticationService.AuthenticateUser(user.Username);

        return Task.FromResult<ActionResult>(Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            expiration = jwtSecurityToken.ValidTo
        }));
    }
}