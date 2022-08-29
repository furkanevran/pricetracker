using Microsoft.Extensions.DependencyInjection;
using pricetracker.extractor;

namespace pricetracker.infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExtractor, AmazonExtractor>();
        return serviceCollection;
    }
}