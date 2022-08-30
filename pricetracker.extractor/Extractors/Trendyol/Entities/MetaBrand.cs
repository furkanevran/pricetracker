using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class MetaBrand
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("beautifiedName")] public string BeautifiedName { get; set; } = null!;
    [JsonPropertyName("isVirtual")] public bool IsVirtual { get; set; }
    [JsonPropertyName("path")] public string Path { get; set; } = null!;
}