using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class WalletRebate
{
    [JsonPropertyName("minPrice")] public int MinPrice { get; set; }
    [JsonPropertyName("maxPrice")] public int MaxPrice { get; set; }
    [JsonPropertyName("rebateRatio")] public double RebateRatio { get; set; }
}