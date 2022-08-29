using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class CrossPromotionAward
{
    [JsonPropertyName("awardType")] public object AwardType { get; set; } = null!;
    [JsonPropertyName("awardValue")] public object AwardValue { get; set; } = null!;
    [JsonPropertyName("contentId")] public int ContentId { get; set; }
    [JsonPropertyName("merchantId")] public int MerchantId { get; set; }
}