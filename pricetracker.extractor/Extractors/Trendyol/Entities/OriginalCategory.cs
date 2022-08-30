using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class OriginalCategory
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("hierarchy")] public string Hierarchy { get; set; } = null!;
    [JsonPropertyName("refundable")] public bool Refundable { get; set; }
    [JsonPropertyName("beautifiedName")] public string BeautifiedName { get; set; } = null!;
    [JsonPropertyName("isVASEnabled")] public bool IsVASEnabled { get; set; }
}