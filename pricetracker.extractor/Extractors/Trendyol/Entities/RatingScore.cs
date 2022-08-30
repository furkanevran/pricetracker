using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class RatingScore
{
    [JsonPropertyName("averageRating")] public double AverageRating { get; set; }
    [JsonPropertyName("totalRatingCount")] public int TotalRatingCount { get; set; }
    [JsonPropertyName("totalCommentCount")] public int TotalCommentCount { get; set; }
}