using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class OriginalPrice
{
    [JsonPropertyName("text")] public string Text { get; set; } = null!;
    [JsonPropertyName("value")] public double Value { get; set; }
}