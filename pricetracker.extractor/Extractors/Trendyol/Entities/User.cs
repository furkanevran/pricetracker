using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class User
{
    [JsonPropertyName("loggedIn")] public bool LoggedIn { get; set; }
    [JsonPropertyName("isBuyer")] public bool IsBuyer { get; set; }
    [JsonPropertyName("isElite")] public bool IsElite { get; set; }
}