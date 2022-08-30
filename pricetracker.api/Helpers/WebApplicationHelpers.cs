using System.Reflection;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints;

namespace PriceTracker.API.Helpers;

public static class WebApplicationHelpers
{
    public static void MapMinimalEndpoints(this WebApplication app)
    {
        var endpointTypes = typeof(IEndpoint).Assembly.GetTypes().Where(t => typeof(IEndpoint).IsAssignableFrom(t) &&
                                                                             !t.IsAbstract &&
                                                                             !t.IsInterface &&
                                                                             t.IsPublic);

        foreach (var endpointType in endpointTypes)
        {
            var pattern = endpointType.GetCustomAttribute<PatternAttribute>()?.Pattern ?? endpointType.Name;
            IEndpoint CreateEndpoint() => (IEndpoint)ActivatorUtilities.CreateInstance(app.Services, endpointType)!;
    
            var endpoint = CreateEndpoint();

            if (endpoint.Delete != null)
                app.MapDelete(pattern, CreateEndpoint().Delete!);
            if (endpoint.Get != null)
                app.MapGet(pattern, CreateEndpoint().Get!);
            if (endpoint.Post != null)
                app.MapPost(pattern, CreateEndpoint().Post!);
            if (endpoint.Put != null)
                app.MapPut(pattern, CreateEndpoint().Put!);
        }
    }
}