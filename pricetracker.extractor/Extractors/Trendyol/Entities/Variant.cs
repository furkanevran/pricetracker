using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Variant
{
    [JsonPropertyName("attributeId")] public int AttributeId { get; set; }
    [JsonPropertyName("attributeName")] public string AttributeName { get; set; } = null!;
    [JsonPropertyName("attributeType")] public string AttributeType { get; set; } = null!;
    [JsonPropertyName("attributeValue")] public string AttributeValue { get; set; } = null!;
    [JsonPropertyName("stamps")] public List<object> Stamps { get; set; } = null!;
    [JsonPropertyName("price")] public Price Price { get; set; } = null!;
    [JsonPropertyName("fulfilmentType")] public string FulfilmentType { get; set; } = null!;
    [JsonPropertyName("attributeBeautifiedValue")]
    public string AttributeBeautifiedValue { get; set; } = null!;
    [JsonPropertyName("isWinner")] public bool IsWinner { get; set; }
    [JsonPropertyName("listingId")] public string ListingId { get; set; } = null!;
    [JsonPropertyName("stock")] public object Stock { get; set; } = null!;
    [JsonPropertyName("sellable")] public bool Sellable { get; set; }
    [JsonPropertyName("availableForClaim")] public bool AvailableForClaim { get; set; }
    [JsonPropertyName("barcode")] public string Barcode { get; set; } = null!;
    [JsonPropertyName("itemNumber")] public int ItemNumber { get; set; }
    [JsonPropertyName("discountedPriceInfo")] public string DiscountedPriceInfo { get; set; } = null!;
    [JsonPropertyName("hasCollectable")] public bool HasCollectable { get; set; }
    [JsonPropertyName("unitInfo")] public UnitInfo UnitInfo { get; set; } = null!;
    [JsonPropertyName("rushDeliveryMerchantListingExist")]
    public bool RushDeliveryMerchantListingExist { get; set; }
    [JsonPropertyName("lowerPriceMerchantListingExist")]
    public bool LowerPriceMerchantListingExist { get; set; }
    [JsonPropertyName("fastDeliveryOptions")] public List<object> FastDeliveryOptions { get; set; } = null!;
}