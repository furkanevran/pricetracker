namespace PriceTracker.Extractor;

public interface IPriceExtractor
{
    bool CanExtract(string url);
    Task<double?> ExtractPrice(string url);
}