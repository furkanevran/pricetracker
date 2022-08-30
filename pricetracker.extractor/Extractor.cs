namespace PriceTracker.Extractor;

public class Extractor : IExtractor
{
    private readonly IEnumerable<IPriceExtractor> _extractors;

    public Extractor(IEnumerable<IPriceExtractor> extractors)
    {
        _extractors = extractors ?? throw new ArgumentNullException(nameof(extractors));
    }
    
    public Task<double?> ExtractPrice(string url)
    {
        foreach (var extractor in _extractors)
        {
            if (extractor.CanExtract(url))
                return extractor.ExtractPrice(url);
        }

        return Task.FromResult((double?)null);
    }
}