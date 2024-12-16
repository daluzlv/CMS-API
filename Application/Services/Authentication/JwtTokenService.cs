using Infrastructure.Identity.Models;
using Infrastructure.Interfaces.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Authentication;

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public string Token(User user)
    {
        var claims = new[]
        {
            new Claim("sub", user.Id),
            new Claim("email", user.Email),
        };

        var secret = configuration["JwtSettings:Secret"];
        var issuer = configuration["JwtSettings:Issuer"];
        var expirationHours = int.Parse(configuration["JwtSettings:ExpirationHours"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.Now.AddHours(expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
