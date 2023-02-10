using System.Text.Json.Serialization;

namespace PriceTracker.API.Entities;

public record ValidationErrorEntry(string? Field, string Message);
public record ValidationErrorResponse([property: JsonPropertyName("errors")] IEnumerable<ValidationErrorEntry> Errors);