using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class HepsiburadaPriceExtractor : IPriceExtractor
{
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.hepsiburada.com/");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var html = await WebHelper.GetString(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        
        var jsonMetadata = doc.DocumentNode
            .Descendants("script")
            .First(script => script.InnerText.Contains("var productModel = "))
            .InnerText.Split('\n').First(line => line.Contains("var productModel = "))
            .Split("var productModel = ")[1];
        
        if (jsonMetadata.EndsWith(";"))
            jsonMetadata = jsonMetadata[..^1];

        return JsonPropertyParser.TryParse<double?>(jsonMetadata, "product", "listings", "price", "amount");
    }
}