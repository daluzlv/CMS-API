using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Infrastructure.Identity.Services;

public class IdentityService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
{
    public async Task<AuthResult> CreateUser(string email, string name, string senha)
    {
        var user = new ApplicationUser(email, name);

        var createUserResult = await _userManager.CreateAsync(user, senha);

        if (!createUserResult.Succeeded)
        {
            var result = new AuthResult(createUserResult.Succeeded);
            result.AddErrors(createUserResult.Errors);
            return result;
        }

        await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.Name, name));

        return new AuthResult(createUserResult.Succeeded);
    }
    public async Task<SignInResult> Login(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        return result;
    }
}
