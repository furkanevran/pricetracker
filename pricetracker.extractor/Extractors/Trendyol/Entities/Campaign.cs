using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Campaign
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("startDate")] public DateTime StartDate { get; set; }
    [JsonPropertyName("endDate")] public DateTime EndDate { get; set; }
    [JsonPropertyName("isMultipleSupplied")] public bool IsMultipleSupplied { get; set; }
    [JsonPropertyName("stockTypeId")] public int StockTypeId { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; } = null!;
    [JsonPropertyName("showTimer")] public bool ShowTimer { get; set; }
}