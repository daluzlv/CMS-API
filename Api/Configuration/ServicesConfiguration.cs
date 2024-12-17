using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalAngularApp",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RoutePrefixConvention("api"));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true; // Formata o JSON
        });

        services.AddEndpointsApiExplorer();
        services.AddSwagger();

        var jwtKey = configuration["JwtSettings:Secret"];
        var jwtIssuer = configuration["JwtSettings:Issuer"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        services.AddAuthorization();

        services.AddDependencyInjection();

        return services;
    }
}
