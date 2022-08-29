using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class CallToAction
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("selector")] public string Selector { get; set; } = null!;
}