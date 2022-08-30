namespace PriceTracker.Extractor;

public interface IWebClient
{
    Task<string> GetString(string url);
}