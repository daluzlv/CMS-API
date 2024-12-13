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
        var handler = new JwtSecurityTokenHandler();

        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email!));
        ci.AddClaim(new Claim(ClaimTypes.Role, "Adm"));

        var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]!));
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = ci,
            Expires = DateTime.UtcNow.AddHours(double.Parse(configuration["JwtSettings:ExpirationHours"]!)),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
