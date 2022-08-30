using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class Value
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
}