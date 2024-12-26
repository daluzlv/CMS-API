using Application.DTOs;
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
public class AuthenticationController(UserManager<User> userManager, ITokenService tokenService) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;

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
        if (user == null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, login.Password))
            return Unauthorized();

        var accessToken = _tokenService.Token(user);
        return Ok(new { accessToken });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(ApiCreateUserDTO dto)
    {
        var validator = dto.Validate();
        if (!validator.IsValid)
        {
            return BadRequest(validator.Errors.ToList());
        }

        var user = new User(dto.Email, dto.Fullname);
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Usuário criado com sucesso!" });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return BadRequest(new { message = "Parâmetros inválidos." });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }

        var decodedToken = Uri.UnescapeDataString(token);
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (result.Succeeded)
        {
            return Ok(new { message = "Email confirmado com sucesso!" });
        }

        return BadRequest(new { message = "Erro ao confirmar email." });
    }
}
