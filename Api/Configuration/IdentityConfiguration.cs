using Infrastructure.Data;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Configuration;

public static class IdentityConfiguration
{
    public static readonly string[] UnmappedIdentityEndpoints = ["/api/auth/login", "/api/auth/refresh", "/api/auth/confirmEmail", "/api/auth/resendConfirmationEmail", "/api/auth/forgotPassword", "/api/auth/resetPassword", "/api/auth/manage/2fa"];

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
        app.Use(async (context, next) =>
        {
            var path = context.Request.Path;

            foreach (var unmappedIdentityEndpoint in UnmappedIdentityEndpoints)
            {
                if (path.StartsWithSegments(unmappedIdentityEndpoint))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Endpoint não disponível");
                    return;
                }
            }

            await next();
        });

        app.MapGroup("/api/auth").MapIdentityApi<User>();

        return app;
    }
}
