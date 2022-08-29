using Microsoft.Extensions.DependencyInjection;
using pricetracker.extractor;
using pricetracker.extractor.Extractors.Amazon;

namespace pricetracker.infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExtractor, AmazonExtractor>();
        return serviceCollection;
    }
}