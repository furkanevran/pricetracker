using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OneOf;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PriceTracker.API;

public class HandleOneOfSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null || context.Type == null)
        {
            return;
        }

        var type = context.Type;
        if (!typeof(IOneOf).IsAssignableFrom(type))
            return;

        var genericArgs = type.GetGenericArguments();
        var asTs = genericArgs.Select((t, i) => new { t, i }).ToDictionary(x => $"AsT{x.i}", x => x.t);

        schema.Properties.Clear();
        var schemaFromRepo = context.SchemaRepository.Schemas.FirstOrDefault(x => x.Value == schema).Key;
        if (schemaFromRepo != null)
            context.SchemaRepository.Schemas.Remove(schemaFromRepo);

        foreach (var (key, value) in asTs)
        {
            if (value.IsArray)
            {
                var elementType = value.GetElementType();
                var name = elementType!.Name;

                if (!context.SchemaRepository.Schemas.ContainsKey(name))
                {
                    var props = context.SchemaGenerator.GenerateSchema(elementType, context.SchemaRepository);
                    context.SchemaRepository.Schemas.Add(name, props);
                }

                var newSchema = new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = name,
                            Type = ReferenceType.Schema
                        }
                    }
                };

                if (!context.SchemaRepository.Schemas.ContainsKey(name+"Array"))
                    context.SchemaRepository.Schemas.Add(name + "Array", newSchema);

                schema.Properties.Add(key, new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Id = name + "Array",
                        Type = ReferenceType.Schema
                    }
                });
            }
            else
            {
                var name = value.Name;

                if (!context.SchemaRepository.Schemas.ContainsKey(name))
                {
                    var props = context.SchemaGenerator.GenerateSchema(value, context.SchemaRepository);
                    context.SchemaRepository.Schemas.Add(name, props);
                }

                schema.Properties.Add(key, new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Id = name,
                        Type = ReferenceType.Schema
                    }
                });
            }
        }
    }
}