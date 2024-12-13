namespace Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAuthorization();
        services.AddSwaggerGen();

        return services;
    }
}
