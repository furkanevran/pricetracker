using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class UnitInfo
{
    [JsonPropertyName("unitPrice")] public bool UnitPrice { get; set; }
    [JsonPropertyName("unitPriceText")] public bool UnitPriceText { get; set; }
}