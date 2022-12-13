using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace PriceTracker.Entities.Providers;

public interface IValidatorProvider
{
    IValidator? GetValidator(Type type);
}


public class ValidatorProvider : IValidatorProvider
{
    private readonly AssemblyScanner.AssemblyScanResult[] validatorTypes;

    public ValidatorProvider(IEnumerable<Assembly> assemblies)
    {
        validatorTypes = AssemblyScanner.FindValidatorsInAssemblies(assemblies).ToArray();
    }

    public IValidator? GetValidator(Type type)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(type);
        var validatorResult = validatorTypes.FirstOrDefault(x => x.InterfaceType == validatorType);

        if (validatorResult != null)
            return Activator.CreateInstance(validatorResult.ValidatorType) as IValidator;

        return null;
    }
}