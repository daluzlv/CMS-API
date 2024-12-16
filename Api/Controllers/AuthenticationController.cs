using Application.DTOs.Auth;
using Infrastructure.Identity.Models;
using Infrastructure.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public AuthenticationController(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Data()
    {
        var email = User.Claims.First(c => c.Type == ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email.Value);

        return Ok(User);
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate(LoginDTO login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            return Unauthorized();

        var accessToken = _tokenService.Token(user);
        return Ok(new { accessToken });
    }
}
