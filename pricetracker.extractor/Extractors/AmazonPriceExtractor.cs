using System.Text.Json;
using HtmlAgilityPack;

namespace PriceTracker.Extractor.Extractors;

public class AmazonPriceExtractor : IPriceExtractor
{
    private readonly IWebClient _webClient;

    public AmazonPriceExtractor(IWebClient webClient)
    {
        _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
    }
    
    public bool CanExtract(string url)
    {
        return url.StartsWith("https://www.amazon.com");
    }

    public async Task<double?> ExtractPrice(string url)
    {
        var html = await _webClient.GetString(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        
        var jsonMetadata =  doc.DocumentNode
            .Descendants("div")
            .First(div => div.HasClass("twister-plus-buying-options-price-data"))
            .InnerText;

        var productModel = JsonSerializer.Deserialize<JsonElement>(jsonMetadata);

        return productModel.GetProperty("priceAmount").GetDouble();
    }
}
