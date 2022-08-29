using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Widget
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("order")] public int Order { get; set; }
}