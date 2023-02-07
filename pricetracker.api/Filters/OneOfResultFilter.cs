using OneOf;

namespace PriceTracker.API.Filters;

public class OneOfResultFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        if (result is not IOneOf oneOf)
            return result;

        var value = oneOf.Value;

        return value;
    }
}