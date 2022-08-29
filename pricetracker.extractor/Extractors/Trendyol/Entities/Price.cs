using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Price
{
    [JsonPropertyName("profitMargin")] public int ProfitMargin { get; set; }
    [JsonPropertyName("discountedPrice")] public DiscountedPrice DiscountedPrice { get; set; } = null!;
    [JsonPropertyName("sellingPrice")] public SellingPrice SellingPrice { get; set; } = null!;
    [JsonPropertyName("originalPrice")] public OriginalPrice OriginalPrice { get; set; } = null!;
    [JsonPropertyName("currency")] public string Currency { get; set; } = null!;
}