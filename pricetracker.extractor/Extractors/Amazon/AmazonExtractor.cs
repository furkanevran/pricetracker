using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors.Amazon;

public class AmazonExtractor : IExtractor
{
    public async Task<double> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        
        var jsonMetadata =  doc.DocumentNode
            .Descendants("div")
            .First(div => div.HasClass("twister-plus-buying-options-price-data"))
            .InnerText;
        
        return JsonPropertyParser.TryParse<double>(jsonMetadata, "priceAmount");
    }
}
