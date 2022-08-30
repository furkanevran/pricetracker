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
            var endpoint = (IEndpoint)ActivatorUtilities.CreateInstance(app.Services, endpointType)!;

            if (endpoint.Delete != null)
                app.MapDelete(pattern, endpoint.Delete);
            if (endpoint.Get != null)
                app.MapGet(pattern, endpoint.Get);
            if (endpoint.Post != null)
                app.MapPost(pattern, endpoint.Post);
            if (endpoint.Put != null)
                app.MapPut(pattern, endpoint.Put);
        }
    }
}