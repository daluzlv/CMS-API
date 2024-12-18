﻿using Application.Interfaces;
using Application.Services;
using Application.Services.Authentication;
using Domain.Interfaces.Repositories.Base;
using Infrastructure.Data.Repositories;
using Infrastructure.Interfaces.Services.Authentication;

namespace Api.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}
