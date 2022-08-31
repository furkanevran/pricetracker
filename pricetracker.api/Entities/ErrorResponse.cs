using System.Text.Json.Serialization;

namespace PriceTracker.API.Entities;

public record ErrorResponse([property: JsonPropertyName("errors")] IEnumerable<string> Errors);