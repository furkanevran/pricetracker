using System.Text.Json;
using HtmlAgilityPack;
using PriceTracker.Extractor.Extractors.Trendyol.Entities;

namespace PriceTracker.Extractor.Extractors.Trendyol;

public class TrendyolExtractor : IExtractor
{
    private static readonly JsonSerializerOptions MetadataSerializerOptions = new(JsonSerializerDefaults.Web);
    
    public async Task<double> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var jsonMetadata =  doc.DocumentNode
            .Descendants("script")
            .First(script => script.InnerText.StartsWith("window.__PRODUCT_DETAIL_APP_INITIAL_STATE__="))
            .InnerText["window.__PRODUCT_DETAIL_APP_INITIAL_STATE__=".Length..]
            .Split(";window.")[0];

        
        var metadata = JsonSerializer.Deserialize<Root>(jsonMetadata, MetadataSerializerOptions)!;
        return metadata.Product.Price.DiscountedPrice.Value;
    }
}