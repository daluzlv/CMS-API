using Infrastructure.Data;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Configuration;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        });

        return services;
    }

    public static WebApplication UseIdentityConfiguration(this WebApplication app)
    {
        app.MapGroup("/api/auth").MapIdentityApi<User>();

        return app;
    }
}
