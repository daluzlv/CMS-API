using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
        services.AddControllers(options =>
        {
            options.Conventions.Add(new RoutePrefixConvention("api"));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwagger();

        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorization();

        return services;
    }
}
