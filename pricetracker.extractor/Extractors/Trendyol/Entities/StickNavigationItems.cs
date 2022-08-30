using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class StickNavigationItems
{
    [JsonPropertyName("PRODUCT_DETAILS")] public PRODUCTDETAILS PRODUCTDETAILS { get; set; } = null!;
    [JsonPropertyName("PRODUCT_REVIEWS")] public PRODUCTREVIEWS PRODUCTREVIEWS { get; set; } = null!;
    [JsonPropertyName("SIMILAR_PRODUCTS")] public SIMILARPRODUCTS SIMILARPRODUCTS { get; set; } = null!;
    [JsonPropertyName("OTHER_MERCHANTS")] public OTHERMERCHANTS OTHERMERCHANTS { get; set; } = null!;
    [JsonPropertyName("CROSS_PRODUCTS")] public CROSSPRODUCTS CROSSPRODUCTS { get; set; } = null!;
    [JsonPropertyName("REPORT_CONTENT")] public REPORTCONTENT REPORTCONTENT { get; set; } = null!;
    [JsonPropertyName("PRODUCT_DESCRIPTION")] public PRODUCTDESCRIPTION PRODUCTDESCRIPTION { get; set; } = null!;
    [JsonPropertyName("SLP")] public SLP SLP { get; set; } = null!;
    [JsonPropertyName("COLLECTION_RECOMMENDATION")]
    public COLLECTIONRECOMMENDATION COLLECTIONRECOMMENDATION { get; set; } = null!;
    [JsonPropertyName("RELATED_CATEGORIES")] public RELATEDCATEGORIES RELATEDCATEGORIES { get; set; } = null!;
}