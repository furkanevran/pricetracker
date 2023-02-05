using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Entities;

[Index(nameof(TrackingProductId), nameof(AddedAt), IsUnique = true, IsDescending = new[] {false, true})]
[Index(nameof(TrackingProductId))]
[Index(nameof(Price))]
[Index(nameof(AddedAt), AllDescending = true)]
public class TrackingProductPrice
{
    [Required] public Guid TrackingProductPriceId { get; set; }

    [Required] public Guid TrackingProductId { get; set; }
    [ForeignKey(nameof(TrackingProductId))] public TrackingProduct TrackingProduct { get; set; } = null!;

    [Required] public double Price { get; set; }

    [Required] public DateTime AddedAt { get; set; }
}

public class TrackingProductPriceValidator : AbstractValidator<TrackingProductPrice>
{
    public TrackingProductPriceValidator()
    {
        RuleFor(x => x.TrackingProductPriceId).NotEmpty();
        RuleFor(x => x.TrackingProductId).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.AddedAt).NotEmpty().LessThan(DateTime.UtcNow);
    }
}