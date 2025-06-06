using Chat.API.Hubs.Abstractions;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace Chat.API.ServiceRegistration
{
    public static class SignalrHubMapper
    {
        public static void MapHubsForAssemblyContains<TMarker>(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(SignalrHubMapper));
            logger.LogInformation("Scanning assembly containing '{marker}'...", typeof(TMarker).Name);

            try
            {
                var assembly = typeof(TMarker).Assembly;
                var hubs = assembly
                    .GetTypes()
                    .Where(
                        t => t.GetCustomAttribute<HubRouteAttribute>() is not null
                        && (t.IsAssignableTo(typeof(Hub)) || t.IsAssignableTo(typeof(Hub<>)))
                    ).ToList();

                logger.LogInformation("{n} hubs were found.", hubs.Count);

                foreach (var hub in hubs)
                    MapHub(hub, app, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to bind hubs.");
                throw;
            }
        }

        private static void MapHub(Type hubType, WebApplication app, ILogger logger)
        {
            var attribute = hubType.GetCustomAttribute<HubRouteAttribute>()!;
            if (attribute.Route is null)
                throw new InvalidOperationException($"Failed to map hub '{hubType.Name}': route argument cant be null.");

            logger.LogInformation("Binding hub '{hub}' to the route '{route}'", hubType.Name, attribute.Route);

            var mapHubMethod = typeof(HubEndpointRouteBuilderExtensions)
                .GetMethod(nameof(HubEndpointRouteBuilderExtensions.MapHub), [typeof(IEndpointRouteBuilder), typeof(string)]) // .MapHub(IEndpointRouteBuilder, string) сигнатура
                ?? throw new InvalidOperationException($"Failed to get metadata for method '{nameof(HubEndpointRouteBuilderExtensions.MapHub)}'."); 

            mapHubMethod = mapHubMethod.MakeGenericMethod(hubType);
            mapHubMethod.Invoke(null, [app, attribute.Route]);
        }
    }
}
