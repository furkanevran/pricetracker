using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class UnitInfo
{
    [JsonPropertyName("unitPrice")] public bool UnitPrice { get; set; }
    [JsonPropertyName("unitPriceText")] public bool UnitPriceText { get; set; }
}