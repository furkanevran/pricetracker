using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints;
using PriceTracker.API.Filters;

namespace PriceTracker.API.Helpers;

public static class WebApplicationHelpers
{
    public static void MapMinimalEndpoints(this WebApplication app)
    {
        static Delegate CreateDelegate(MethodInfo method)
        {
            var funcType = Expression.GetDelegateType(method.GetParameters().Select(p => p.ParameterType).Append(method.ReturnType).ToArray());
            return Delegate.CreateDelegate(funcType, method);
        }
        
        IEnumerable<Delegate> GetActions<T>(IReflect endpointType) where T : Attribute
        {
            var foundMethods = endpointType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.GetCustomAttribute<T>() != null);
            
            return foundMethods.Select(CreateDelegate).ToArray();
        }
        
        var endpointTypes = typeof(IEndpoint).Assembly.GetTypes().Where(t => typeof(IEndpoint).IsAssignableFrom(t) &&
                                                                             !t.IsAbstract &&
                                                                             !t.IsInterface &&
                                                                             t.IsPublic);

        foreach (var endpointType in endpointTypes)
        {
            var pattern = endpointType.GetCustomAttribute<PatternAttribute>()?.Pattern ?? endpointType.Name;

            foreach (var post in GetActions<HttpPostAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Post.Method}, post).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var get in GetActions<HttpGetAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Get.Method}, get).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var put in GetActions<HttpPutAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Put.Method}, put).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var delete in GetActions<HttpDeleteAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Delete.Method}, delete).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var patch in GetActions<HttpPatchAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Patch.Method}, patch).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var head in GetActions<HttpHeadAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Head.Method}, head).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var options in GetActions<HttpOptionsAttribute>(endpointType))
                app.MapMethods(pattern, new[] {HttpMethod.Options.Method}, options).AddEndpointFilter<ValidateEntityFilter>();
            
            // if (endpoint.Delete != null)
            //     app.MapDelete(pattern, endpoint.Delete).AddEndpointFilter<ValidateEntityFilter>();
            // if (endpoint.Get != null)
            //     app.MapGet(pattern, endpoint.Get).AddEndpointFilter<ValidateEntityFilter>();
            // if (endpoint.Post != null)
            //     app.MapPost(pattern, endpoint.Post).AddEndpointFilter<ValidateEntityFilter>();
            // if (endpoint.Put != null)
            //     app.MapPut(pattern, endpoint.Put).AddEndpointFilter<ValidateEntityFilter>();
        }
    }
}