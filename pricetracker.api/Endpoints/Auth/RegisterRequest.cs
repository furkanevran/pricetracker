using FluentValidation;

namespace PriceTracker.API.Endpoints.User;

public record RegisterRequest(string EMail, string Username, string Password, string PasswordConfirm);

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(32);
        RuleFor(x => x.EMail).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.PasswordConfirm).NotEmpty().MinimumLength(8).Equal(x => x.Password);
    }
}