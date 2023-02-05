using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Entities;

[Index(nameof(EMail), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(TokenVersion))]
public class User
{
    [Required] public Guid UserId { get; set; }
    [Required] public string EMail { get; set; } = null!;
    [Required] public string Username { get; set; } = null!;
    [Required] public string PasswordHash { get; set; } = null!;
    [Required] public string PasswordSalt { get; set; } = null!;
    [Required] public int TokenVersion { get; set; }

    public List<UserProduct>? UserProducts { get; set; }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(32);
        RuleFor(x => x.EMail).NotEmpty().EmailAddress();
        RuleFor(x => x.PasswordHash).NotEmpty();
        RuleFor(x => x.PasswordSalt).NotEmpty();
        RuleFor(x => x.TokenVersion).NotEmpty().GreaterThan(0);
    }
}