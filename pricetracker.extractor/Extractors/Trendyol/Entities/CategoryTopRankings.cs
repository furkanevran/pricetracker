using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class CategoryTopRankings
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("order")] public int Order { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; } = null!;
    [JsonPropertyName("text")] public string Text { get; set; } = null!;
}