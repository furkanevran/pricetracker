using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class RELATEDCATEGORIES
{
    [JsonPropertyName("translateKey")] public string TranslateKey { get; set; } = null!;
    [JsonPropertyName("selector")] public string Selector { get; set; } = null!;
}