using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors;
using PriceTracker.Persistence;

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

    public static IServiceCollection AddAppDbContext(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null)
    {
        serviceCollection
            .AddEntityFrameworkSqlite()
            .AddDbContext<AppDbContext>((provider, builder) =>
            {
                // builder.UseSqlite("Data Source=app.db", options => { options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); });
                optionsAction?.Invoke(provider, builder);
            });

        return serviceCollection;
    }
}