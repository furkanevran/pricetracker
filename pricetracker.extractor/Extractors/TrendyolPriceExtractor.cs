using System.Text.Json;
using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class TrendyolPriceExtractor : IPriceExtractor
{
    private readonly IWebClient _webClient;

    public TrendyolPriceExtractor(IWebClient webClient)
    {
        _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
    }
    
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.trendyol.com/");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var html = await _webClient.GetString(url);
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var jsonMetadata =  doc.DocumentNode
            .Descendants("script")
            .First(script => script.InnerText.StartsWith("window.__PRODUCT_DETAIL_APP_INITIAL_STATE__="))
            .InnerText["window.__PRODUCT_DETAIL_APP_INITIAL_STATE__=".Length..]
            .Split(";window.")[0];


        var productModel = JsonSerializer.Deserialize<JsonElement>(jsonMetadata);

        return productModel.GetProperty("product").GetProperty("price").GetProperty("discountedPrice").GetProperty("value").GetDouble();
    }
}