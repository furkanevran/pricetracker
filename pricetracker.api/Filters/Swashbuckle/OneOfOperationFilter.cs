using System.Reflection;
using Microsoft.OpenApi.Models;
using OneOf;
using PriceTracker.API.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PriceTracker.API.Filters.Swashbuckle;

public class OneOfOperationFilter : IOperationFilter
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

            var searchTypeName = value.Name.Split('`', StringSplitOptions.RemoveEmptyEntries)[0];
            if (typeof(IStatusCodeHttpResult).IsAssignableFrom(value))
            {
                var typedResults = typeof(TypedResults).GetMethods(BindingFlags.Public | BindingFlags.Static);
                var typedResult = typedResults.FirstOrDefault(x => x.ReturnType.Name == searchTypeName);
                if (typedResult != null)
                {
                    fromTypedResults = true;

                    var invokedTypedResultObj = (IStatusCodeHttpResult)typedResult.Invoke(null, null)!;
                    var statusCode = invokedTypedResultObj.GetType().GetProperty("StatusCode")!.GetValue(invokedTypedResultObj);
                    if (statusCode is int i)
                        code = i;
                }
            }

            var response = new OpenApiResponse
            {
                Description = value.Name,
                Content = new Dictionary<string, OpenApiMediaType>()
            };

            if (fromTypedResults)
            {
                context.SchemaRepository.Schemas[name].Properties.Clear();

                if (code == 400)
                {
                    if (!context.SchemaRepository.Schemas.TryGetValue(name, out var existingBadRequestSchema) ||
                        existingBadRequestSchema.Properties.Count == 0)
                        context.SchemaRepository.Schemas[name] = context.SchemaGenerator.GenerateSchema(typeof(ValidationErrorResponse), context.SchemaRepository);

                    response.Content.Add("application/json", new OpenApiMediaType
                    {
                        Schema = context.SchemaRepository.Schemas[name]
                    });
                }
            }
            else
                response.Content.Add("application/json", new OpenApiMediaType
                {
                    Schema = context.SchemaRepository.Schemas[name]
                });

            operation.Responses.Add(code.ToString(), response);
        }
    }
}