using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PriceTracker.API.Filters.Swashbuckle;

public class OneOfDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var oneOfSchemas = context.SchemaRepository.Schemas.Where(x => x.Key.EndsWith("OneOf")).ToList();
        foreach (var (key, value) in oneOfSchemas)
            if (value.Properties.Any(x => x.Key.StartsWith("AsT") && x.Key.Length > 3))
                context.SchemaRepository.Schemas.Remove(key);
    }
}