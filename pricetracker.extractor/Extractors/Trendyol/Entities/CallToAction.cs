using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class CallToAction
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("selector")] public string Selector { get; set; } = null!;
}