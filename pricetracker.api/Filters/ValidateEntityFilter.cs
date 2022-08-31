using FluentValidation;
using PriceTracker.API.Endpoints;
using PriceTracker.API.Entities;

namespace PriceTracker.API.Filters;

public class ValidateEntityFilter : IEndpointFilter
{
    private static (Type ValidatorType, Type EntityType)[] _validators;
    static ValidateEntityFilter()
    {
        var openGenericType = typeof(IValidator<>);

        _validators = (from type in typeof(Program).Assembly.GetTypes()
            where !type.IsAbstract && !type.IsGenericTypeDefinition
            let interfaces = type.GetInterfaces()
            let genericInterfaces = interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
            let matchingInterface = genericInterfaces.FirstOrDefault()
            where matchingInterface != null
            select (type, matchingInterface.GetGenericArguments()[0])).ToArray();
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var argument in context.Arguments)
        {
            var argType = argument!.GetType();
            var validator = _validators.FirstOrDefault(validator => validator.EntityType == argType);

            if (validator.ValidatorType == null) continue;
            var validatorInstance = (IValidator)ActivatorUtilities.CreateInstance(context.HttpContext.RequestServices, validator.ValidatorType);

            var result = await validatorInstance.ValidateAsync(new ValidationContext<object?>(Convert.ChangeType(argument, validator.EntityType)));
            if (!result.IsValid)
                return Results.BadRequest(new ErrorResponse(result.Errors.Select(x => x.ErrorMessage)));
        }
        
        return await next(context);
    }
}