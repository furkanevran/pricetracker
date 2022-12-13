using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Entities.Providers;

public interface IValidatorProvider
{
    IValidator? GetValidator(Type type, IServiceProvider? serviceProvider = null);
}

public class ValidatorProvider : IValidatorProvider
{
    private readonly AssemblyScanner.AssemblyScanResult[] _validatorTypes;

    public ValidatorProvider(IEnumerable<Assembly> assemblies)
    {
        _validatorTypes = AssemblyScanner.FindValidatorsInAssemblies(assemblies).ToArray();
    }

    public IValidator? GetValidator(Type type, IServiceProvider? serviceProvider = null)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(type);
        var validatorResult = _validatorTypes.FirstOrDefault(x => x.InterfaceType == validatorType);

        if (validatorResult != null)
            return (serviceProvider == null ?
                Activator.CreateInstance(validatorResult.ValidatorType) :
                ActivatorUtilities.CreateInstance(serviceProvider, validatorResult.ValidatorType)) as IValidator;

        return null;
    }
}