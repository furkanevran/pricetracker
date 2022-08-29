using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Root
{
    [JsonPropertyName("product")] public Product Product { get; set; } = null!;
    [JsonPropertyName("htmlContent")] public object HtmlContent { get; set; } = null!;
    [JsonPropertyName("user")] public User User { get; set; } = null!;
    [JsonPropertyName("configuration")] public Configuration Configuration { get; set; } = null!;
}