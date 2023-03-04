using FluentValidation;

namespace PriceTracker.API.Endpoints;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeHttps<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(url => url.StartsWith("https://")).WithMessage("URL must be HTTPS");
    }

    public static IRuleBuilderOptions<T, string> MustBeValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL");
    }

    public static IRuleBuilderOptions<T, string> MustBeValidHttpsUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.MustBeHttps().MustBeValidUrl();
    }
}