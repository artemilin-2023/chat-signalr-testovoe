using Chat.Contracts.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contracts
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddContracts(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, UtcDateTimeProvider>();

            return services;
        }
    }
}
