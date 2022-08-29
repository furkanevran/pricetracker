using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Brand
{
    [JsonPropertyName("isVirtual")] public bool IsVirtual { get; set; }
    [JsonPropertyName("beautifiedName")] public string BeautifiedName { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("path")] public string Path { get; set; } = null!;
}