using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class AllVariant
{
    [JsonPropertyName("itemNumber")] public int ItemNumber { get; set; }
    [JsonPropertyName("value")] public string Value { get; set; } = null!;
    [JsonPropertyName("inStock")] public bool InStock { get; set; }
    [JsonPropertyName("currency")] public string Currency { get; set; } = null!;
    [JsonPropertyName("barcode")] public string Barcode { get; set; } = null!;
    [JsonPropertyName("price")] public int Price { get; set; }
}