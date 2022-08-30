using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors.Amazon;
using PriceTracker.Extractor.Extractors.Trendyol;

namespace PriceTracker.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExtractor, AmazonExtractor>();
        serviceCollection.AddSingleton<IExtractor, TrendyolExtractor>();
        return serviceCollection;
    }
}