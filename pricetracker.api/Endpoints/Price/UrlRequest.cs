using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace PriceTracker.API.Endpoints.Price;

public class UrlRequest
{
    [JsonPropertyName("url"), Required] public string Url { get; set; } = null!;
}

public class UrlRequestValidator : AbstractValidator<UrlRequest>
{
    public UrlRequestValidator()
    {
        RuleFor(x => x.Url).NotEmpty();
    }
}

