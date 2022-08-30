using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceTracker.API;

class UrlRequest
{
    [JsonPropertyName("url"), Required] public string Url { get; set; } = null!;
}