using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Entities;

[Index(nameof(UserId), nameof(TrackingProductId), IsUnique = true)]
[Index(nameof(IsEnabled), AllDescending = true)]
public class UserAlert
{
    [Required] public Guid UserAlertId { get; set; }
    [Required] public Guid UserId { get; set; }
    [Required] public Guid TrackingProductId { get; set; }

    [Required] public double LessOrEqualThan { get; set; }
    [Required] public AlertStrategies EnabledAlertStrategies { get; set; }
    public bool IsEnabled { get; set; } = true;
}

public class UserAlertValidator : AbstractValidator<UserAlert>
{
    public UserAlertValidator()
    {
        RuleFor(x => x.UserAlertId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TrackingProductId).NotEmpty();

        RuleFor(x => x.LessOrEqualThan).GreaterThan(0);
    }
}