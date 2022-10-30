using FluentValidation;

namespace PriceTracker.API.Endpoints.User;

public record LoginRequest(string Username, string Password);

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(32);
        RuleFor(x => x.Password).NotEmpty();
    }
}