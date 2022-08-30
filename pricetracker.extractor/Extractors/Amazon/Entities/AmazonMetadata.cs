using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Amazon.Entities;

public class AmazonMetadata
{
    [JsonPropertyName("displayPrice")] public string DisplayPrice { get; set; } = null!;
    [JsonPropertyName("priceAmount")] public double PriceAmount { get; set; }
    [JsonPropertyName("currencySymbol")] public string CurrencySymbol { get; set; } = null!;
    [JsonPropertyName("integerValue")] public string IntegerValue { get; set; } = null!;
    [JsonPropertyName("decimalSeparator")] public string DecimalSeparator { get; set; } = null!;
    [JsonPropertyName("fractionalValue")] public string FractionalValue { get; set; } = null!;
    [JsonPropertyName("symbolPosition")] public string SymbolPosition { get; set; } = null!;
    [JsonPropertyName("hasSpace")] public bool HasSpace { get; set; }
    [JsonPropertyName("showFractionalPartIfEmpty")] public bool ShowFractionalPartIfEmpty { get; set; }
    [JsonPropertyName("offerListingId")] public string OfferListingId { get; set; } = null!;
    [JsonPropertyName("locale")] public string Locale { get; set; } = null!;
    [JsonPropertyName("buyingOptionType")] public string BuyingOptionType { get; set; } = null!;
}