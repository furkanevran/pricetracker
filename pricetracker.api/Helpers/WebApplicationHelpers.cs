using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
        
        static (string Template, Delegate Handler)[] GetActions<T>(string baseTemplate, IReflect endpointType) where T : HttpMethodAttribute
        {
            var foundMethods = endpointType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.GetCustomAttribute<T>() != null);
            
            return foundMethods.Select(method =>
            {
                var template = baseTemplate;
                if (method.GetCustomAttribute<T>()!.Template is { } templateSuffix)
                    template += "/" + templateSuffix;
                
                return (template, CreateDelegate(method));
            }).ToArray();
        }
        
        var endpointTypes = typeof(IEndpoint).Assembly.GetTypes().Where(t => typeof(IEndpoint).IsAssignableFrom(t) &&
                                                                             !t.IsAbstract &&
                                                                             !t.IsInterface &&
                                                                             t.IsPublic);

        foreach (var endpointType in endpointTypes)
        {
            var pattern = endpointType.GetCustomAttribute<PatternAttribute>()?.Pattern ?? endpointType.Name;

            foreach (var post in GetActions<HttpPostAttribute>(pattern, endpointType))
                app.MapMethods(post.Template, new[] {HttpMethod.Post.Method}, post.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var get in GetActions<HttpGetAttribute>(pattern, endpointType))
                app.MapMethods(get.Template, new[] {HttpMethod.Get.Method}, get.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var put in GetActions<HttpPutAttribute>(pattern, endpointType))
                app.MapMethods(put.Template, new[] {HttpMethod.Put.Method}, put.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var delete in GetActions<HttpDeleteAttribute>(pattern, endpointType))
                app.MapMethods(delete.Template, new[] {HttpMethod.Delete.Method}, delete.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var patch in GetActions<HttpPatchAttribute>(pattern, endpointType))
                app.MapMethods(patch.Template, new[] {HttpMethod.Patch.Method}, patch.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var head in GetActions<HttpHeadAttribute>(pattern, endpointType))
                app.MapMethods(head.Template, new[] {HttpMethod.Head.Method}, head.Handler).AddEndpointFilter<ValidateEntityFilter>();
            
            foreach (var options in GetActions<HttpOptionsAttribute>(pattern, endpointType))
                app.MapMethods(options.Template, new[] {HttpMethod.Options.Method}, options.Handler).AddEndpointFilter<ValidateEntityFilter>();
        }
    }
}