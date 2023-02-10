using FluentValidation;
using PriceTracker.API.Entities;

namespace PriceTracker.API.Filters;

public class ValidateEntityFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var argument in context.Arguments)
        {
            var argType = argument!.GetType();
            var validator = context.HttpContext.RequestServices.GetService(typeof(IValidator<>).MakeGenericType(argType)) as IValidator;

            if (validator == null) continue;

            var result = await validator.ValidateAsync(new ValidationContext<object?>(Convert.ChangeType(argument, argType)));
            if (!result.IsValid)
                return Results.BadRequest(new ValidationErrorResponse(result.Errors.Select(x => new ValidationErrorEntry(x.PropertyName, x.ErrorMessage))));
        }

        try
        {
            return await next(context);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new ValidationErrorResponse(ex.Errors.Select(x => new ValidationErrorEntry(x.PropertyName, x.ErrorMessage))));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ValidationErrorResponse(new []{ new ValidationErrorEntry(null, ex.Message) }));
        }
    }
}