using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Gender
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
}