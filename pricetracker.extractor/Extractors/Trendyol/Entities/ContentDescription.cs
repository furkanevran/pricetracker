using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class ContentDescription
{
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("bold")] public bool Bold { get; set; }
}