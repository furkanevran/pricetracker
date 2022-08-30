using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class MerchantBadge
{
    [JsonPropertyName("webImageUrl")] public string WebImageUrl { get; set; } = null!;
    [JsonPropertyName("mobileImageUrl")] public string MobileImageUrl { get; set; } = null!;
    [JsonPropertyName("type")] public string Type { get; set; } = null!;
}