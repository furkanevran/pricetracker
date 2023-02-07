using System.Reflection;
using Microsoft.OpenApi.Models;
using OneOf;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PriceTracker.API;

public class HandleOneOfOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodInfo = context.MethodInfo;
        var returnType = methodInfo.ReturnType;

        if (returnType.IsGenericType)
        {
            var genericType = returnType.GetGenericTypeDefinition();
            if (genericType == typeof(Task<>) ||
                genericType == typeof(Nullable<>))
                returnType = returnType.GetGenericArguments()[0];
        }

        if (!typeof(IOneOf).IsAssignableFrom(returnType))
            return;

        var genericArgs = returnType.GetGenericArguments();
        var asTs = genericArgs.Select((t, i) => new { t, i }).ToDictionary(x => $"AsT{x.i}", x => x.t);

        operation.Responses.Clear();

        foreach (var (_, value) in asTs)
        {
            var name = value.Name;
            if (value.IsArray)
                name = value.GetElementType()!.Name + "Array";

            var code = 200;
            var fromTypedResults = false;

            if (typeof(IStatusCodeHttpResult).IsAssignableFrom(value))
            {
                var statusCodeProperty = value.GetProperty("StatusCode", BindingFlags.Public | BindingFlags.Instance);
                if (statusCodeProperty != null)
                {
                    var typedResults = typeof(TypedResults).GetMethods(BindingFlags.Public | BindingFlags.Static);
                    var typedResult = typedResults.FirstOrDefault(x => x.ReturnType == value);
                    fromTypedResults = true;

                    var statusCode = statusCodeProperty.GetValue(typedResult!.Invoke(null, null));
                    if (statusCode is int i)
                        code = i;
                }
            }

            var response = new OpenApiResponse
            {
                Description = value.Name,
                Content = new Dictionary<string, OpenApiMediaType>()
            };

            if (!fromTypedResults)
            {
                response.Content.Add("application/json", new OpenApiMediaType
                {
                    Schema = context.SchemaRepository.Schemas[name]
                });
            }

            operation.Responses.Add(code.ToString(), response);
        }
    }
}