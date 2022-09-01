using JetBrains.Annotations;

namespace PriceTracker.API.Endpoints;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class AbstractValidator<T> : FluentValidation.AbstractValidator<T>
{
}