using System;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Entities;

public class ConsumedRefreshToken
{
    [Required]
    public Guid ConsumedRefreshTokenId { get; set; }
}