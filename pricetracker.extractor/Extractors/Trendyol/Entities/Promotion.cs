using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class Promotion
{
    [JsonPropertyName("promotionRemainingTime")]
    public string PromotionRemainingTime { get; set; } = null!;
    [JsonPropertyName("type")] public int Type { get; set; }
    [JsonPropertyName("text")] public string Text { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("promotionDiscountType")]
    public string PromotionDiscountType { get; set; } = null!;
    [JsonPropertyName("link")] public string Link { get; set; } = null!;
}