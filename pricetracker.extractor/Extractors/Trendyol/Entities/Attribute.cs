using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Attribute
{
    [JsonPropertyName("key")] public Key Key { get; set; } = null!;
    [JsonPropertyName("value")] public Value Value { get; set; } = null!;
    [JsonPropertyName("starred")] public bool Starred { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("mediaUrls")] public List<object> MediaUrls { get; set; } = null!;
}