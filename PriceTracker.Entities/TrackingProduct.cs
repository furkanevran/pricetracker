using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace PriceTracker.Entities;

public class TrackingProduct
{
    [Required] public Guid TrackingProductId { get; set; }

    [Required] public string Url { get; set; }

    [Required] public Guid AddedByUserId { get; set; }
    [ForeignKey(nameof(AddedByUserId))] public User AddedByUser { get; set; } = null!;

    [Required] public DateTime AddedAt { get; set; }

    public List<TrackingProductPrice>? TrackingProductPrices { get; set; }
}

public class TrackingUrlValidator : AbstractValidator<TrackingProduct>
{
    public TrackingUrlValidator()
    {
        RuleFor(x => x.TrackingProductId).NotEmpty();

        RuleFor(x => x.Url)
            .Must(url => url.StartsWith("https://")).WithMessage("URL must be HTTPS")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL")
            .NotEmpty();

        RuleFor(x => x.AddedByUserId).NotEmpty();

        RuleFor(x => x.AddedAt).NotEmpty().LessThan(DateTime.UtcNow);
    }
}