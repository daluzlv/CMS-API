using Application.Services.Authentication;
using Infrastructure.Interfaces.Services.Authentication;

namespace Api.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
