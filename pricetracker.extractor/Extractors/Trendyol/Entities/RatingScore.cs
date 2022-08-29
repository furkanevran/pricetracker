using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class RatingScore
{
    [JsonPropertyName("averageRating")] public double AverageRating { get; set; }
    [JsonPropertyName("totalRatingCount")] public int TotalRatingCount { get; set; }
    [JsonPropertyName("totalCommentCount")] public int TotalCommentCount { get; set; }
}