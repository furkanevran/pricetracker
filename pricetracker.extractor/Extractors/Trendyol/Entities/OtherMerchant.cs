using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class OtherMerchant
{
    [JsonPropertyName("url")] public string Url { get; set; } = null!;
    [JsonPropertyName("reviewsUrl")] public string ReviewsUrl { get; set; } = null!;
    [JsonPropertyName("questionsUrl")] public string QuestionsUrl { get; set; } = null!;
    [JsonPropertyName("promotions")] public List<object> Promotions { get; set; } = null!;
    [JsonPropertyName("discountedPriceInfo")] public string DiscountedPriceInfo { get; set; } = null!;
    [JsonPropertyName("isSellable")] public bool IsSellable { get; set; }
    [JsonPropertyName("isBasketDiscount")] public bool IsBasketDiscount { get; set; }
    [JsonPropertyName("hasStock")] public bool HasStock { get; set; }
    [JsonPropertyName("price")] public Price Price { get; set; } = null!;
    [JsonPropertyName("isFreeCargo")] public bool IsFreeCargo { get; set; }
    [JsonPropertyName("merchant")] public Merchant Merchant { get; set; } = null!;
    [JsonPropertyName("deliveryInformation")] public DeliveryInformation DeliveryInformation { get; set; } = null!;
    [JsonPropertyName("cargoRemainingDays")] public int CargoRemainingDays { get; set; }
    [JsonPropertyName("isBlacklist")] public bool IsBlacklist { get; set; }
}