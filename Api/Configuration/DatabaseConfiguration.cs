﻿using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("DefaultDatabase");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

        return services;
    }
}
