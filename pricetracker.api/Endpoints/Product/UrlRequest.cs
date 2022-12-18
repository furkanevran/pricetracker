using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace PriceTracker.API.Endpoints.Price;

public class AddProductRequest
{
    [JsonPropertyName("url"), Required] public string Url { get; set; } = null!;
    [JsonPropertyName("tag"), MaxLength(120)] public string Tag { get; set; } = null!;
}

public class UrlRequestValidator : AbstractValidator<AddProductRequest>
{
    public UrlRequestValidator()
    {
        RuleFor(x => x.Url)
            .Must(url => url.StartsWith("https://")).WithMessage("URL must be HTTPS")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL")
            .NotEmpty();

        RuleFor(x => x.Tag)
            .MaximumLength(120);
    }
}

