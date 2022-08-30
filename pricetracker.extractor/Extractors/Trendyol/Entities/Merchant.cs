using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class Merchant
{
    [JsonPropertyName("isSearchableMerchant")] public bool IsSearchableMerchant { get; set; }
    [JsonPropertyName("stickers")] public List<object> Stickers { get; set; } = null!;
    [JsonPropertyName("merchantBadges")] public List<MerchantBadge> MerchantBadges { get; set; } = null!;
    [JsonPropertyName("merchantMarkers")] public List<object> MerchantMarkers { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("officialName")] public string OfficialName { get; set; } = null!;
    [JsonPropertyName("cityName")] public string CityName { get; set; } = null!;
    [JsonPropertyName("centralRegistrationSystemNumber")]
    public string CentralRegistrationSystemNumber { get; set; } = null!;
    [JsonPropertyName("registeredEmailAddress")]
    public string RegisteredEmailAddress { get; set; } = null!;
    [JsonPropertyName("taxNumber")] public string TaxNumber { get; set; } = null!;
    [JsonPropertyName("sellerScore")] public double SellerScore { get; set; }
    [JsonPropertyName("sellerScoreColor")] public string SellerScoreColor { get; set; } = null!;
    [JsonPropertyName("deliveryProviderName")] public string DeliveryProviderName { get; set; } = null!;
    [JsonPropertyName("corporateInvoiceApplicable")]
    public bool CorporateInvoiceApplicable { get; set; }
    [JsonPropertyName("sellerLink")] public string SellerLink { get; set; } = null!;
}