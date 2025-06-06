using Chat.API.Extensions;
using Chat.API.Middlewares;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Chat.API.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddCustomSwaggerGen();

            services.AddAuthentificationRules(configuration);

            services.AddCustomLogging(configuration);

            services.AddSignalR(config => config.AddFilter<SignalrExceptionHandler>());

            return services;
        }

        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSerilog(Log.Logger);
            return services;
        }
    }
}
