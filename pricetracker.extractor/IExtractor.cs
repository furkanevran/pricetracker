namespace PriceTracker.Extractor;

public  interface IExtractor
{
    Task<double?> ExtractPrice(string url);
}