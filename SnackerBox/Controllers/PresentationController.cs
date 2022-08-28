using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackerBox.Services.Interfaces;

namespace SnackerBox.Controllers;

[ApiController]
public class PresentationController : ControllerBase
{
    private readonly ISnackableParsingService _snackableParsingService;
    private readonly ILogger<PresentationController> _logger;
    private readonly IUserService _userService;

    public PresentationController(ISnackableParsingService snackableParsingService,
        IUserService userService,
        ILogger<PresentationController> logger)
    {
        _snackableParsingService = snackableParsingService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("Snackable")]
    public async Task<IActionResult> Get([FromQuery] string fileId)
    {
        var user = HttpContext.User;

        if (user.HasClaim(c => c.Type == ClaimTypes.Role) &&
            user.Claims.First(x => x.Type == ClaimTypes.Role).Value == "API User")
        {
            var name = user.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var userDto = _userService.FindUserByUsername(name);

            if (userDto == null)
            {
                _logger.LogDebug($"Request token used by user does not correspond to any users");
                return Unauthorized(
                    "Your request token does not correspond to any Snacker Users. Contact System Administrators if you believe this is a fault");
            }

            var isValidFile = await _snackableParsingService.FindFinishedFile(fileId);

            if (isValidFile)
            {
                var file = await _snackableParsingService.FindMetadata(fileId);
                if (file.User == userDto.Username)
                {
                    return Ok(file);
                }

                return Unauthorized("You are not permitted to access this file");
            }
        }

        return BadRequest("The fileId of your file is invalid or the file is in processing/has failed processing");
    }
}