using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
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

        static IEnumerable<(string Template, Delegate Handler)> GetActions<T>(string baseTemplate, IReflect endpointType) where T : HttpMethodAttribute
        {
            var foundMethods = endpointType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.GetCustomAttribute<T>() != null);

            return foundMethods.Select(method =>
            {
                var template = baseTemplate;
                if (method.GetCustomAttribute<T>()!.Template is { } templateSuffix)
                    template += "/" + templateSuffix;
                
                return (template, CreateDelegate(method));
            });
        }
        
        var endpointTypes = typeof(IEndpoint).Assembly.GetTypes().Where(t => typeof(IEndpoint).IsAssignableFrom(t) &&
                                                                             t is {IsAbstract: false, IsInterface: false, IsPublic: true});

        foreach (var endpointType in endpointTypes)
        {
            var authorizeAttr = endpointType.GetCustomAttribute<AuthorizeAttribute>() as IAuthorizeData;
            var pattern = endpointType.GetCustomAttribute<TemplateAttribute>()?.Template ?? endpointType.Name;

            void MapMethods<T>() where T : HttpMethodAttribute
            {
                foreach (var post in GetActions<T>(pattern, endpointType))
                {
                    var builder = app.MapMethods(post.Template, new[] {HttpMethod.Post.Method}, post.Handler)
                        .AddEndpointFilter<ValidateEntityFilter>();

                    if (authorizeAttr != null)
                        builder.WithMetadata(authorizeAttr);
                }

            }

            MapMethods<HttpPostAttribute>();
            MapMethods<HttpGetAttribute>();
            MapMethods<HttpPutAttribute>();
            MapMethods<HttpDeleteAttribute>();
            MapMethods<HttpPatchAttribute>();
            MapMethods<HttpHeadAttribute>();
            MapMethods<HttpOptionsAttribute>();
        }
    }
}