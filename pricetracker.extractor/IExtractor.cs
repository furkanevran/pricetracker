namespace pricetracker.extractor;

public interface IExtractor
{
    Task<double> ExtractPrice(string url);
}