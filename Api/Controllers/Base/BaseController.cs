using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.Base;

[ApiController]
public class BaseController(UserManager<User> userManager) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;

    protected async Task<Guid?> GetUserIdAsync()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (email == null) return null;

        var user = await _userManager.FindByEmailAsync(email.Value);

        return Guid.Parse(user!.Id);
    }
}
