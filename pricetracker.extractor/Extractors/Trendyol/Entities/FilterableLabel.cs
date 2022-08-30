using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class FilterableLabel
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = null!;
    [JsonPropertyName("visibleAgg")] public bool VisibleAgg { get; set; }
}