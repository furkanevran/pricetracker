using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace PriceTracker.API.Endpoints.User;

public record GetProductsRequest : IPaginatedRequest
{
    [JsonPropertyName("url")] public string Url { get; set; } = null!;
    [JsonPropertyName("tag"), MaxLength(120)] public string Tag { get; set; } = null!;
    [JsonPropertyName("take")] public int Take { get; set; }
    [JsonPropertyName("skip")] public int Skip { get; set; }
}

public class UrlRequestValidator : AbstractValidator<GetProductsRequest>
{
    public UrlRequestValidator()
    {
        RuleFor(x => x.Url)
            .Must(url => url.StartsWith("https://")).WithMessage("URL must be HTTPS")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL")
            .When(x => !string.IsNullOrEmpty(x.Url));

        RuleFor(x => x.Tag)
            .MaximumLength(120);
    }
}
