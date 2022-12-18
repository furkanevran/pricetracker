using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace PriceTracker.Entities;

public class UserProduct
{
    [Required] public Guid UserProductId { get; set; }
    [MaxLength(120)] public string Tag { get; set; }

    [Required] public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User User { get; set; }

    [Required] public Guid TrackingProductId { get; set; }
    [ForeignKey(nameof(TrackingProductId))] public TrackingProduct TrackingProduct { get; set; } = null!;
}

public class UserProductValidator : AbstractValidator<UserProduct>
{
    public UserProductValidator()
    {
        RuleFor(x => x.UserProductId).NotEmpty();
        RuleFor(x => x.Tag).MaximumLength(120);
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TrackingProductId).NotEmpty();
    }
}