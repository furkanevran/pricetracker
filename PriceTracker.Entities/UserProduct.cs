using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Entities;

[Index(nameof(Tag))]
[Index(nameof(UserId), nameof(TrackingProductId), IsUnique = true)]
public class UserProduct
{
    [Required] public Guid UserProductId { get; set; }
    [MaxLength(120)] public string Tag { get; set; } = null!;

    [Required] public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User User { get; set; } = null!;

    [Required] public Guid TrackingProductId { get; set; }
    [ForeignKey(nameof(TrackingProductId))] public TrackingProduct TrackingProduct { get; set; } = null!;
}

public class UserProductValidator : AbstractValidator<UserProduct>
{
    public UserProductValidator()
    {
        RuleFor(x => x.UserProductId).NotEmpty();
        RuleFor(x => x.Tag).MaximumLength(120).NotNull();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TrackingProductId).NotEmpty();
    }
}