using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Entities.Providers;
using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors;

namespace PriceTracker.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IWebClient, WebClient>();
        serviceCollection.AddSingleton<IPriceExtractor, AmazonPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, TrendyolPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, HepsiburadaPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, WatsonsPriceExtractor>();

        serviceCollection.AddSingleton<IExtractor, Extractor.Extractor>();

        serviceCollection.AddSingleton<AmazonPriceExtractor, AmazonPriceExtractor>();
        serviceCollection.AddSingleton<TrendyolPriceExtractor, TrendyolPriceExtractor>();
        serviceCollection.AddSingleton<HepsiburadaPriceExtractor, HepsiburadaPriceExtractor>();
        serviceCollection.AddSingleton<WatsonsPriceExtractor, WatsonsPriceExtractor>();

        serviceCollection.AddHttpClient("Extractor");

        return serviceCollection;
    }

    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        serviceCollection.AddValidatorsFromAssemblies(assemblies);
        serviceCollection.AddSingleton<IValidatorProvider>(new ValidatorProvider(assemblies));
        return serviceCollection;
    }

    // public static IServiceCollection AddAppDbContext(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null)
    // {
    //     serviceCollection
    //         .AddEntityFrameworkSqlite()
    //         .AddDbContext<AppDbContext>((provider, builder) =>
    //         {
    //             // builder.UseSqlite("Data Source=app.db", options => { options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); });
    //             optionsAction?.Invoke(provider, builder);
    //         });
    //
    //     return serviceCollection;
    // }
}