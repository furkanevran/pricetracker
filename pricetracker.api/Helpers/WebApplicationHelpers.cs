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
            var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType);

            var funcType = Expression.GetDelegateType(methodParameterTypes.Distinct().Append(method.ReturnType).ToArray());
            return Delegate.CreateDelegate(funcType, method);
        }

        static IEnumerable<(string Template, Delegate Handler, IEnumerable<string> SupportedMethods)>
            GetActions<T>(string baseTemplate, IReflect endpointType) where T : HttpMethodAttribute
        {
            var foundMethods = endpointType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(m => (Method: m, Attribute: m.GetCustomAttribute<T>())).Where(x => x.Attribute is not null);

            return foundMethods.Select(tuple =>
            {
                var (method, attribute) = tuple;

                var template = baseTemplate;
                if (attribute!.Template is { } templateSuffix)
                    template += "/" + templateSuffix;
                
                return (template, CreateDelegate(method), attribute.HttpMethods);
            });
        }
        
        var endpointTypes = typeof(IEndpoint).Assembly.GetTypes().Where(t => typeof(IEndpoint).IsAssignableFrom(t) &&
                                                                             t is {IsAbstract: false, IsInterface: false, IsPublic: true});

        var metadataTypes = new[]
        {
            typeof(AuthorizeAttribute),
            typeof(TagsAttribute)
        };

        foreach (var endpointType in endpointTypes)
        {
            var metadata = endpointType.GetCustomAttributes().Where(x => metadataTypes.Contains(x.GetType()));
            var pattern = endpointType.GetCustomAttribute<TemplateAttribute>()?.Template ?? endpointType.Name;

            void MapMethods<T>() where T : HttpMethodAttribute
            {
                foreach (var method in GetActions<T>(pattern, endpointType))
                {
                    var builder = app.MapMethods(method.Template, method.SupportedMethods, method.Handler)
                        .AddEndpointFilter<ValidateEntityFilter>()
                        .AddEndpointFilter<OneOfResultFilter>();

                    foreach (var attribute in metadata)
                        builder.WithMetadata(attribute);
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