using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class WatsonsExtractor : IExtractor
{
    private const string URL = "https://www.watsons.com.tr/api/v2/wtctr/products/{0}";
    public async Task<double> ExtractPrice(string url)
    {
        var html = await WebHelper.GetString(url);
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var productCode = doc.DocumentNode
            .Descendants("e2-product-reviews-list")
            .First()
            .Attributes["product-code"]
            .Value;
            
        var json = await WebHelper.GetString(string.Format(URL, productCode));

        return 
            JsonPropertyParser.TryParse<double?>(json, "otherPrices", "value") ??
            JsonPropertyParser.TryParse<double>(json, "price", "value");
    }
}