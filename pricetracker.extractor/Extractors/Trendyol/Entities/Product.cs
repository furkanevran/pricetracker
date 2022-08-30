using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class Product
{
    [JsonPropertyName("attributes")] public List<Attribute> Attributes { get; set; } = null!;
    [JsonPropertyName("alternativeVariants")] public List<object> AlternativeVariants { get; set; } = null!;
    [JsonPropertyName("variants")] public List<Variant> Variants { get; set; } = null!;
    [JsonPropertyName("otherMerchants")] public List<OtherMerchant> OtherMerchants { get; set; } = null!;
    [JsonPropertyName("campaign")] public Campaign Campaign { get; set; } = null!;
    [JsonPropertyName("category")] public Category Category { get; set; } = null!;
    [JsonPropertyName("brand")] public Brand Brand { get; set; } = null!;
    [JsonPropertyName("color")] public object Color { get; set; } = null!;
    [JsonPropertyName("metaBrand")] public MetaBrand MetaBrand { get; set; } = null!;
    [JsonPropertyName("showVariants")] public bool ShowVariants { get; set; }
    [JsonPropertyName("showSexualContent")] public bool ShowSexualContent { get; set; }
    [JsonPropertyName("brandCategoryBanners")] public List<object> BrandCategoryBanners { get; set; } = null!;
    [JsonPropertyName("allVariants")] public List<AllVariant> AllVariants { get; set; } = null!;
    [JsonPropertyName("otherMerchantVariants")]
    public List<object> OtherMerchantVariants { get; set; } = null!;
    [JsonPropertyName("isThereAnyCorporateInvoiceInOtherMerchants")]
    public bool IsThereAnyCorporateInvoiceInOtherMerchants { get; set; }
    [JsonPropertyName("categoryTopRankings")] public CategoryTopRankings CategoryTopRankings { get; set; } = null!;
    [JsonPropertyName("isVasEnabled")] public bool IsVasEnabled { get; set; }
    [JsonPropertyName("originalCategory")] public OriginalCategory OriginalCategory { get; set; } = null!;
    [JsonPropertyName("landings")] public List<object> Landings { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("productCode")] public string ProductCode { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("nameWithProductCode")] public string NameWithProductCode { get; set; } = null!;
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("contentDescriptions")] public List<ContentDescription> ContentDescriptions { get; set; } = null!;
    [JsonPropertyName("productGroupId")] public int ProductGroupId { get; set; }
    [JsonPropertyName("tax")] public int Tax { get; set; }
    [JsonPropertyName("businessUnit")] public string BusinessUnit { get; set; } = null!;
    [JsonPropertyName("maxInstallment")] public int MaxInstallment { get; set; }
    [JsonPropertyName("gender")] public Gender Gender { get; set; } = null!;
    [JsonPropertyName("url")] public string Url { get; set; } = null!;
    [JsonPropertyName("images")] public List<string> Images { get; set; } = null!;
    [JsonPropertyName("isSellable")] public bool IsSellable { get; set; }
    [JsonPropertyName("isBasketDiscount")] public bool IsBasketDiscount { get; set; }
    [JsonPropertyName("hasStock")] public bool HasStock { get; set; }
    [JsonPropertyName("price")] public Price Price { get; set; } = null!;
    [JsonPropertyName("isFreeCargo")] public bool IsFreeCargo { get; set; }
    [JsonPropertyName("promotions")] public List<Promotion> Promotions { get; set; } = null!;
    [JsonPropertyName("merchant")] public Merchant Merchant { get; set; } = null!;
    [JsonPropertyName("deliveryInformation")] public DeliveryInformation DeliveryInformation { get; set; } = null!;
    [JsonPropertyName("cargoRemainingDays")] public int CargoRemainingDays { get; set; }
    [JsonPropertyName("isMarketplace")] public bool IsMarketplace { get; set; }
    [JsonPropertyName("productStamps")] public List<object> ProductStamps { get; set; } = null!;
    [JsonPropertyName("hasHtmlContent")] public bool HasHtmlContent { get; set; }
    [JsonPropertyName("favoriteCount")] public int FavoriteCount { get; set; }
    [JsonPropertyName("uxLayout")] public string UxLayout { get; set; } = null!;
    [JsonPropertyName("isDigitalGood")] public bool IsDigitalGood { get; set; }
    [JsonPropertyName("isRunningOut")] public bool IsRunningOut { get; set; }
    [JsonPropertyName("scheduledDelivery")] public bool ScheduledDelivery { get; set; }
    [JsonPropertyName("ratingScore")] public RatingScore RatingScore { get; set; } = null!;
    [JsonPropertyName("showStarredAttributes")]
    public bool ShowStarredAttributes { get; set; }
    [JsonPropertyName("reviewsUrl")] public string ReviewsUrl { get; set; } = null!;
    [JsonPropertyName("questionsUrl")] public string QuestionsUrl { get; set; } = null!;
    [JsonPropertyName("sellerQuestionEnabled")]
    public bool SellerQuestionEnabled { get; set; }
    [JsonPropertyName("sizeChartUrl")] public string SizeChartUrl { get; set; } = null!;
    [JsonPropertyName("sizeExpectationAvailable")]
    public bool SizeExpectationAvailable { get; set; }
    [JsonPropertyName("crossPromotionAward")] public CrossPromotionAward CrossPromotionAward { get; set; } = null!;
    [JsonPropertyName("rushDeliveryMerchantListingExist")]
    public bool RushDeliveryMerchantListingExist { get; set; }
    [JsonPropertyName("lowerPriceMerchantListingExist")]
    public bool LowerPriceMerchantListingExist { get; set; }
    [JsonPropertyName("showValidFlashSales")] public bool ShowValidFlashSales { get; set; }
    [JsonPropertyName("showExpiredFlashSales")]
    public bool ShowExpiredFlashSales { get; set; }
    [JsonPropertyName("walletRebate")] public WalletRebate WalletRebate { get; set; } = null!;
    [JsonPropertyName("isArtWork")] public bool IsArtWork { get; set; }
    [JsonPropertyName("buyMorePayLessPromotions")]
    public List<object> BuyMorePayLessPromotions { get; set; } = null!;
    [JsonPropertyName("filterableLabels")] public List<FilterableLabel> FilterableLabels { get; set; } = null!;
}