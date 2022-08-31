using System.Reflection;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints;
using PriceTracker.API.Filters;

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
                app.MapDelete(pattern, endpoint.Delete).AddEndpointFilter<ValidateEntityFilter>();
            if (endpoint.Get != null)
                app.MapGet(pattern, endpoint.Get).AddEndpointFilter<ValidateEntityFilter>();
            if (endpoint.Post != null)
                app.MapPost(pattern, endpoint.Post).AddEndpointFilter<ValidateEntityFilter>();
            if (endpoint.Put != null)
                app.MapPut(pattern, endpoint.Put).AddEndpointFilter<ValidateEntityFilter>();
        }
    }
}