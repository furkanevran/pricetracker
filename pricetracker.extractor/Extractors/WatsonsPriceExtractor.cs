using System.Text.Json;
using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class WatsonsPriceExtractor : IPriceExtractor
{
    private const string URL = "https://www.watsons.com.tr/api/v2/wtctr/products/{0}";
    private readonly IWebClient _webClient;

    public WatsonsPriceExtractor(IWebClient webClient)
    {
        _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
    }
    
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.watsons.com");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var html = await _webClient.GetString(url);
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var productCode = doc.DocumentNode
            .Descendants("e2-product-reviews-list")
            .First()
            .Attributes["product-code"]
            .Value;
            
        var json = await _webClient.GetString(string.Format(URL, productCode));

        var productModel = JsonSerializer.Deserialize<JsonElement>(json);

        var allOptions = productModel.GetProperty("baseOptions").EnumerateArray().SelectMany(x => x.GetProperty("options").EnumerateArray());
        var optionsInStock = allOptions.Where(x => x.GetProperty("stock").GetProperty("stockLevel").GetInt32() > 0);
        return optionsInStock.Min(x => x.GetProperty("priceData").GetProperty("value").GetDouble());
    }
}