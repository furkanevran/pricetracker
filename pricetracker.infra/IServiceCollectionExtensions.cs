using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors.Amazon;

namespace PriceTracker.Infra;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExtractor, AmazonExtractor>();
        return serviceCollection;
    }
}