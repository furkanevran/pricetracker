using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceTracker.API.Endpoints.Price;

public class UrlRequest
{
    [JsonPropertyName("url"), Required] public string Url { get; set; } = null!;
}