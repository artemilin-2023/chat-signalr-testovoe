using Chat.Common;
using Chat.Infrastructure.Abstractions.Auth;
using Chat.Infrastructure.Abstractions.Data;
using Chat.Infrastructure.Auth;
using Chat.Infrastructure.Data.Configurations.RegistrationExtension;
using Chat.Infrastructure.Data.Contexts;
using Chat.Infrastructure.Data.Repositories;
using Chat.Infrastructure.ServiceRegistration.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.ServiceRegistration;

public static class ServiceRegistrator
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .LoadOptions(configuration)
            .AddDbContext(configuration)
            .AddRedisCache(configuration)
            .AddAuth()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection LoadOptions(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetAndSaveConfiguration<JwtOptions>(services);

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDataAccess<DataContext, DataContextConfigurator>();

        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Redis options are not configured");

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<ITokenStorage, TokenStorage>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    private static TConfiguration GetAndSaveConfiguration<TConfiguration>(this IConfiguration configuration, IServiceCollection? services = null)
        where TConfiguration : class
    {
        var configSection = configuration.GetSection(typeof(TConfiguration).Name);
        (configSection is null).ThenThrow(new InvalidOperationException($"Configuration for {typeof(TConfiguration).Name} is not set"));

        var config = configSection!.Get<TConfiguration>();
        (config is null).ThenThrow(new InvalidOperationException($"Configuration for {typeof(TConfiguration).Name} is not set"));

        if (services is null)
            return config!;
        services.Configure<TConfiguration>(configSection!);

        return config!;
    }
}