using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors;

namespace PriceTracker.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPriceExtractor, AmazonPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, TrendyolPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, HepsiburadaPriceExtractor>();
        serviceCollection.AddSingleton<IPriceExtractor, WatsonsPriceExtractor>();
        serviceCollection.AddSingleton<IExtractor, Extractor.Extractor>();
        
        return serviceCollection;
    }
}