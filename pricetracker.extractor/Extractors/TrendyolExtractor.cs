using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class TrendyolExtractor : IExtractor
{
    public async Task<double> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var jsonMetadata =  doc.DocumentNode
            .Descendants("script")
            .First(script => script.InnerText.StartsWith("window.__PRODUCT_DETAIL_APP_INITIAL_STATE__="))
            .InnerText["window.__PRODUCT_DETAIL_APP_INITIAL_STATE__=".Length..]
            .Split(";window.")[0];


        return JsonPropertyParser.TryParse<double>(jsonMetadata, "product", "price", "discountedPrice", "value");
    }
}