using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace PriceTracker.Entities;

public class ConsumedRefreshToken
{
    [Required] public Guid ConsumedRefreshTokenId { get; set; }

    [Required] public DateTime ExpiresAt { get; set; }
}

public class ConsumedRefreshTokenValidator : AbstractValidator<ConsumedRefreshToken>
{
    public ConsumedRefreshTokenValidator()
    {
        RuleFor(x => x.ConsumedRefreshTokenId).NotEmpty();
        RuleFor(x => x.ExpiresAt).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
    }
}