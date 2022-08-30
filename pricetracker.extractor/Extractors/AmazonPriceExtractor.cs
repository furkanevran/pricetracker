using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class AmazonPriceExtractor : IPriceExtractor
{
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.amazon.com");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        
        var jsonMetadata =  doc.DocumentNode
            .Descendants("div")
            .First(div => div.HasClass("twister-plus-buying-options-price-data"))
            .InnerText;
        
        return JsonPropertyParser.TryParse<double?>(jsonMetadata, "priceAmount");
    }
}
