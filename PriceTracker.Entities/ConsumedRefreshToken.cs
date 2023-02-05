using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Entities;

[Index(nameof(ConsumedRefreshTokenId), IsUnique = true)]
[Index(nameof(ExpiresAt))]
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