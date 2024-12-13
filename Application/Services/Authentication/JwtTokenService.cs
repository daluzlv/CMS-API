using Infrastructure.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Authentication;

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public string Token(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim(ClaimTypes.Role, "Adm")
        };

        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!));
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: "",
            claims,
            expires: DateTime.Now.AddHours(double.Parse(configuration["JwtSettings:ExpirationHours"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
