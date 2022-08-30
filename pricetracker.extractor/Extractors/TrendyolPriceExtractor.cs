using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class TrendyolPriceExtractor : IPriceExtractor
{
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.trendyol.com/");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var jsonMetadata =  doc.DocumentNode
            .Descendants("script")
            .First(script => script.InnerText.StartsWith("window.__PRODUCT_DETAIL_APP_INITIAL_STATE__="))
            .InnerText["window.__PRODUCT_DETAIL_APP_INITIAL_STATE__=".Length..]
            .Split(";window.")[0];


        return JsonPropertyParser.TryParse<double?>(jsonMetadata, "product", "price", "discountedPrice", "value");
    }
}