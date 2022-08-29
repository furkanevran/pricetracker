using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Discover
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("link")] public string Link { get; set; } = null!;
    [JsonPropertyName("image")] public string Image { get; set; } = null!;
}