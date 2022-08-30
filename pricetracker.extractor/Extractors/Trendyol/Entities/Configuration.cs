using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class Configuration
{
    [JsonPropertyName("htmlContentCssUrl")] public string HtmlContentCssUrl { get; set; } = null!;
    [JsonPropertyName("htmlContentJsUrl")] public string HtmlContentJsUrl { get; set; } = null!;
    [JsonPropertyName("storefrontId")] public int StorefrontId { get; set; }
    [JsonPropertyName("showPaymentMethodsAndShippings")]
    public bool ShowPaymentMethodsAndShippings { get; set; }
    [JsonPropertyName("showDiscountInformation")]
    public bool ShowDiscountInformation { get; set; }
    [JsonPropertyName("isDynamicRender")] public bool IsDynamicRender { get; set; }
    [JsonPropertyName("widgets")] public List<Widget> Widgets { get; set; } = null!;
    [JsonPropertyName("maxWidgetCount")] public int MaxWidgetCount { get; set; }
    [JsonPropertyName("callToActions")] public List<CallToAction> CallToActions { get; set; } = null!;
    [JsonPropertyName("discovers")] public List<Discover> Discovers { get; set; } = null!;
    [JsonPropertyName("discoverWidgetTitle")] public string DiscoverWidgetTitle { get; set; } = null!;
    [JsonPropertyName("stickNavigationItemsOrder")]
    public List<string> StickNavigationItemsOrder { get; set; } = null!;
    [JsonPropertyName("stickNavigationItems")] public StickNavigationItems StickNavigationItems { get; set; } = null!;
    [JsonPropertyName("isInternational")] public bool IsInternational { get; set; }
    [JsonPropertyName("culture")] public string Culture { get; set; } = null!;
    [JsonPropertyName("cdnUrl")] public string CdnUrl { get; set; } = null!;
    [JsonPropertyName("claimInfoText")] public string ClaimInfoText { get; set; } = null!;
    [JsonPropertyName("questionsAndAnswersEnabled")]
    public bool QuestionsAndAnswersEnabled { get; set; }
    [JsonPropertyName("ratingReviewEnabled")] public bool RatingReviewEnabled { get; set; }
    [JsonPropertyName("otherMerchantsEnabled")]
    public bool OtherMerchantsEnabled { get; set; }
    [JsonPropertyName("ratingReviewLikesEnabled")]
    public bool RatingReviewLikesEnabled { get; set; }
    [JsonPropertyName("newRatingSummaryEnabled")]
    public bool NewRatingSummaryEnabled { get; set; }
    [JsonPropertyName("redirectReviewsPageEnabled")]
    public bool RedirectReviewsPageEnabled { get; set; }
    [JsonPropertyName("languageCode")] public string LanguageCode { get; set; } = null!;
    [JsonPropertyName("sizeCharts")] public List<List<string>> SizeCharts { get; set; } = null!;
    [JsonPropertyName("enhancedEcommerceEnabled")]
    public bool EnhancedEcommerceEnabled { get; set; }
    [JsonPropertyName("addReviewEnabled")] public bool AddReviewEnabled { get; set; }
    [JsonPropertyName("reviewReportAbuseEnabled")]
    public bool ReviewReportAbuseEnabled { get; set; }
    [JsonPropertyName("showImageOnProductCommentsEnabled")]
    public bool ShowImageOnProductCommentsEnabled { get; set; }
    [JsonPropertyName("crossProductsTitle")] public string CrossProductsTitle { get; set; } = null!;
    [JsonPropertyName("crossProductsEnabled")] public bool CrossProductsEnabled { get; set; }
    [JsonPropertyName("recommendationEnabled")]
    public bool RecommendationEnabled { get; set; }
    [JsonPropertyName("productGroupEnabled")] public bool ProductGroupEnabled { get; set; }
    [JsonPropertyName("htmlContentEnabled")] public bool HtmlContentEnabled { get; set; }
    [JsonPropertyName("productAttributesEnabled")]
    public bool ProductAttributesEnabled { get; set; }
    [JsonPropertyName("publishCriteriaEnabled")]
    public bool PublishCriteriaEnabled { get; set; }
    [JsonPropertyName("sellerAreaEnabled")] public bool SellerAreaEnabled { get; set; }
    [JsonPropertyName("sellerPointLowLimit")] public int SellerPointLowLimit { get; set; }
    [JsonPropertyName("sizeChartUrl")] public string SizeChartUrl { get; set; } = null!;
    [JsonPropertyName("productDetailMetaDescription")]
    public string ProductDetailMetaDescription { get; set; } = null!;
    [JsonPropertyName("schemaJsonEnabled")] public bool SchemaJsonEnabled { get; set; }
    [JsonPropertyName("siteAddress")] public string SiteAddress { get; set; } = null!;
    [JsonPropertyName("notifyMeEnabled")] public bool NotifyMeEnabled { get; set; }
    [JsonPropertyName("notifyMeCount")] public int NotifyMeCount { get; set; }
    [JsonPropertyName("recommendationAbTestValue")]
    public string RecommendationAbTestValue { get; set; } = null!;
    [JsonPropertyName("lastProductCountAbTestValue")]
    public string LastProductCountAbTestValue { get; set; } = null!;
    [JsonPropertyName("recoCardAbTestValue")] public string RecoCardAbTestValue { get; set; } = null!;
    [JsonPropertyName("abTestingCookieName")] public string AbTestingCookieName { get; set; } = null!;
    [JsonPropertyName("clientSideReviewsEnabled")]
    public bool ClientSideReviewsEnabled { get; set; }
    [JsonPropertyName("clientSideHtmlContentEnabled")]
    public bool ClientSideHtmlContentEnabled { get; set; }
    [JsonPropertyName("alternativeVariantsEnabled")]
    public bool AlternativeVariantsEnabled { get; set; }
    [JsonPropertyName("relatedCategoryEnabled")]
    public bool RelatedCategoryEnabled { get; set; }
    [JsonPropertyName("relatedCategoryAbTestVariant")]
    public string RelatedCategoryAbTestVariant { get; set; } = null!;
    [JsonPropertyName("relatedCategoryAbTestValue")]
    public string RelatedCategoryAbTestValue { get; set; } = null!;
    [JsonPropertyName("relatedCategoryTitleTooltipThreshold")]
    public int RelatedCategoryTitleTooltipThreshold { get; set; }
    [JsonPropertyName("relatedCategoryCountLimit")]
    public int RelatedCategoryCountLimit { get; set; }
    [JsonPropertyName("relatedCategoryImageVirtualBrandIds")]
    public List<int> RelatedCategoryImageVirtualBrandIds { get; set; } = null!;
    [JsonPropertyName("relatedCategoryVirtualBrandImagePath")]
    public string RelatedCategoryVirtualBrandImagePath { get; set; } = null!;
    [JsonPropertyName("defaultBrandCategoryCombinationImageUrl")]
    public string DefaultBrandCategoryCombinationImageUrl { get; set; } = null!;
    [JsonPropertyName("legalRequirementCacheDuration")]
    public int LegalRequirementCacheDuration { get; set; }
    [JsonPropertyName("digitalGoodsDeliveryText")]
    public string DigitalGoodsDeliveryText { get; set; } = null!;
    [JsonPropertyName("recoCrossCustomStampsEnabled")]
    public bool RecoCrossCustomStampsEnabled { get; set; }
    [JsonPropertyName("canShowSizeChartButton")]
    public bool CanShowSizeChartButton { get; set; }
    [JsonPropertyName("productDetailImprovedBreadcrumbEnabled")]
    public bool ProductDetailImprovedBreadcrumbEnabled { get; set; }
    [JsonPropertyName("sellerShippingEnabled")]
    public bool SellerShippingEnabled { get; set; }
    [JsonPropertyName("productDetailReportAbuseEnabled")]
    public bool ProductDetailReportAbuseEnabled { get; set; }
    [JsonPropertyName("productDetailReportAbuseItems")]
    public string ProductDetailReportAbuseItems { get; set; } = null!;
    [JsonPropertyName("scheduledDeliveryWarningMessage")]
    public string ScheduledDeliveryWarningMessage { get; set; } = null!;
    [JsonPropertyName("installmentStampAmountText")]
    public string InstallmentStampAmountText { get; set; } = null!;
    [JsonPropertyName("starredAttributesLimit")]
    public int StarredAttributesLimit { get; set; }
    [JsonPropertyName("sellerStoreLinkEnabled")]
    public bool SellerStoreLinkEnabled { get; set; }
    [JsonPropertyName("featuredCardFavButtonEnabled")]
    public bool FeaturedCardFavButtonEnabled { get; set; }
    [JsonPropertyName("openReviewModalEnabled")]
    public bool OpenReviewModalEnabled { get; set; }
    [JsonPropertyName("installmentCountToDisplay")]
    public int InstallmentCountToDisplay { get; set; }
    [JsonPropertyName("currencySymbol")] public string CurrencySymbol { get; set; } = null!;
    [JsonPropertyName("newSearchEnabled")] public bool NewSearchEnabled { get; set; }
    [JsonPropertyName("stampType")] public string StampType { get; set; } = null!;
    [JsonPropertyName("memberGwUrl")] public string MemberGwUrl { get; set; } = null!;
    [JsonPropertyName("wishListUrl")] public string WishListUrl { get; set; } = null!;
    [JsonPropertyName("getNotifyMePreferencesFromMemberGw")]
    public bool GetNotifyMePreferencesFromMemberGw { get; set; }
    [JsonPropertyName("publicProductGwUrl")] public string PublicProductGwUrl { get; set; } = null!;
    [JsonPropertyName("publicSdcProductGwUrl")]
    public string PublicSdcProductGwUrl { get; set; } = null!;
    [JsonPropertyName("publicMdcProductGwUrl")]
    public string PublicMdcProductGwUrl { get; set; } = null!;
    [JsonPropertyName("publicMdcRecoGwUrl")] public string PublicMdcRecoGwUrl { get; set; } = null!;
    [JsonPropertyName("productRecommendationVersion")]
    public int ProductRecommendationVersion { get; set; }
    [JsonPropertyName("productDetailSimilarProductsButtonAbTest")]
    public string ProductDetailSimilarProductsButtonAbTest { get; set; } = null!;
    [JsonPropertyName("publicSdcCheckoutGwUrl")]
    public string PublicSdcCheckoutGwUrl { get; set; } = null!;
    [JsonPropertyName("publicMdcCheckoutGwUrl")]
    public string PublicMdcCheckoutGwUrl { get; set; } = null!;
    [JsonPropertyName("addToBasketOnCheckoutgwEnabled")]
    public bool AddToBasketOnCheckoutgwEnabled { get; set; }
    [JsonPropertyName("collectionRecommendationEnabled")]
    public bool CollectionRecommendationEnabled { get; set; }
    [JsonPropertyName("showCargoRemainingDays")]
    public bool ShowCargoRemainingDays { get; set; }
    [JsonPropertyName("pdpLinearVariantsEnabled")]
    public bool PdpLinearVariantsEnabled { get; set; }
    [JsonPropertyName("pdpPromotionImageAbTestEnabled")]
    public bool PdpPromotionImageAbTestEnabled { get; set; }
    [JsonPropertyName("showLinearAlternativeVariants")]
    public bool ShowLinearAlternativeVariants { get; set; }
    [JsonPropertyName("sellerStoreUrl")] public string SellerStoreUrl { get; set; } = null!;
    [JsonPropertyName("minSellerFollowerCount")]
    public int MinSellerFollowerCount { get; set; }
    [JsonPropertyName("showSellerFollowerCount")]
    public bool ShowSellerFollowerCount { get; set; }
    [JsonPropertyName("questionCountToShow")] public int QuestionCountToShow { get; set; }
    [JsonPropertyName("collectableCouponsEnabled")]
    public bool CollectableCouponsEnabled { get; set; }
    [JsonPropertyName("publicSdcCouponGwUrl")] public string PublicSdcCouponGwUrl { get; set; } = null!;
    [JsonPropertyName("publicMdcCouponGwUrl")] public string PublicMdcCouponGwUrl { get; set; } = null!;
    [JsonPropertyName("showInstallmentCampaign")]
    public bool ShowInstallmentCampaign { get; set; }
    [JsonPropertyName("vasServiceEnabled")] public bool VasServiceEnabled { get; set; }
    [JsonPropertyName("showCrossPromotions")] public bool ShowCrossPromotions { get; set; }
    [JsonPropertyName("showFastMerchant")] public bool ShowFastMerchant { get; set; }
    [JsonPropertyName("showWalletRebate")] public bool ShowWalletRebate { get; set; }
    [JsonPropertyName("showDigitalGoodsRebate")]
    public bool ShowDigitalGoodsRebate { get; set; }
    [JsonPropertyName("pudoBannerMoreInformationText")]
    public string PudoBannerMoreInformationText { get; set; } = null!;
    [JsonPropertyName("pudoBannerEnabled")] public bool PudoBannerEnabled { get; set; }
    [JsonPropertyName("topCategoryRankingUIAbTestEnabled")]
    public bool TopCategoryRankingUIAbTestEnabled { get; set; }
    [JsonPropertyName("featuredPriceBoxAbTestEnabled")]
    public bool FeaturedPriceBoxAbTestEnabled { get; set; }
    [JsonPropertyName("showAllQuestionsAbTestEnabled")]
    public bool ShowAllQuestionsAbTestEnabled { get; set; }
    [JsonPropertyName("sizeExpectationFeatureEnabled")]
    public bool SizeExpectationFeatureEnabled { get; set; }
    [JsonPropertyName("showFlashSales")] public bool ShowFlashSales { get; set; }
    [JsonPropertyName("flashSalesTimeSlots")] public List<int> FlashSalesTimeSlots { get; set; } = null!;
    [JsonPropertyName("publicMdcSocialGwUrl")] public string PublicMdcSocialGwUrl { get; set; } = null!;
    [JsonPropertyName("publicSdcSocialGwUrl")] public string PublicSdcSocialGwUrl { get; set; } = null!;
    [JsonPropertyName("minInstallmentAmountEnabled")]
    public bool MinInstallmentAmountEnabled { get; set; }
    [JsonPropertyName("minInstallmentAmountMinPrice")]
    public int MinInstallmentAmountMinPrice { get; set; }
    [JsonPropertyName("minInstallmentAmountMinInstallment")]
    public int MinInstallmentAmountMinInstallment { get; set; }
    [JsonPropertyName("showCheapSeller")] public bool ShowCheapSeller { get; set; }
    [JsonPropertyName("stickersEnabled")] public bool StickersEnabled { get; set; }
    [JsonPropertyName("promotionImageEnabled")]
    public bool PromotionImageEnabled { get; set; }
    [JsonPropertyName("addToCollectionEnabled")]
    public bool AddToCollectionEnabled { get; set; }
    [JsonPropertyName("productGroupSantralUrl")]
    public string ProductGroupSantralUrl { get; set; } = null!;
    [JsonPropertyName("promotionImageSantralUrl")]
    public string PromotionImageSantralUrl { get; set; } = null!;
    [JsonPropertyName("publicWebSfxProductRecommendationServiceUrl")]
    public string PublicWebSfxProductRecommendationServiceUrl { get; set; } = null!;
    [JsonPropertyName("publicMdcContractServiceUrl")]
    public string PublicMdcContractServiceUrl { get; set; } = null!;
    [JsonPropertyName("thresholdDayForLongTermDelivery")]
    public int ThresholdDayForLongTermDelivery { get; set; }
    [JsonPropertyName("infoForLongTermDelivery")]
    public string InfoForLongTermDelivery { get; set; } = null!;
    [JsonPropertyName("priceBoxV2Enable")] public bool PriceBoxV2Enable { get; set; }
    [JsonPropertyName("buyMorePayLessPromotionsText")]
    public string BuyMorePayLessPromotionsText { get; set; } = null!;
    [JsonPropertyName("buyMorePayLessPromotionsEnabled")]
    public bool BuyMorePayLessPromotionsEnabled { get; set; }
    [JsonPropertyName("corporateInvoiceEnabled")]
    public bool CorporateInvoiceEnabled { get; set; }
    [JsonPropertyName("tomorrowShippingEnabled")]
    public bool TomorrowShippingEnabled { get; set; }
    [JsonPropertyName("pdpStickyHeaderNavigationAbTestEnabled")]
    public bool PdpStickyHeaderNavigationAbTestEnabled { get; set; }
    [JsonPropertyName("featuredPriceTooltipEnabled")]
    public bool FeaturedPriceTooltipEnabled { get; set; }
    [JsonPropertyName("featuredPriceTooltipText")]
    public string FeaturedPriceTooltipText { get; set; } = null!;
    [JsonPropertyName("pdpVideoEnabled")] public bool PdpVideoEnabled { get; set; }
    [JsonPropertyName("mediaCenterSantralUrl")]
    public string MediaCenterSantralUrl { get; set; } = null!;
    [JsonPropertyName("pdpReportReasons")] public string PdpReportReasons { get; set; } = null!;
}