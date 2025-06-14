﻿using Chat.Application.Abstractions.Services;
using Chat.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices()
                .LoadConfiguratoins(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IChatRoomService, ChatRoomService>()
                .AddScoped<IMessageService, MessageService>();

            return services;
        }

        private static IServiceCollection LoadConfiguratoins(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
