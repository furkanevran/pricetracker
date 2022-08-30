using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

class UrlRequest
{
    [JsonPropertyName("url"), Required] public string Url { get; set; } = null!;
}