using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PriceTracker.API.Attributes;
using PriceTracker.Entities;
using PriceTracker.Extractor;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

[Tags("User Product")]
[Template("/user/add-product")]
[Authorize]
public class UserAddProductEndpoint : IEndpoint
{
    public record AddProductRequest([property: Required] string Url, [property: MaxLength(120)] string Tag = "");

    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Url)
                .MustBeValidHttpsUrl()
                .NotEmpty();

            RuleFor(x => x.Tag)
                .MaximumLength(120)
                .NotNull();
        }
    }

    [HttpPost]
    [Authorize]
    public static async Task<OneOf<Ok, UnprocessableEntity>> AddProduct([FromBody] AddProductRequest addProductRequest,
        IExtractor extractor,
        IAuthService authService,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId()!.Value;

        var trackingProduct = await dbContext.TrackingProducts
            .FirstOrDefaultAsync(x => x.Url == addProductRequest.Url, cancellationToken: cancellationToken);

        if (trackingProduct == null)
        {
            var price = await extractor.ExtractPrice(addProductRequest.Url);
            if (price == null)
                return TypedResults.UnprocessableEntity();

            trackingProduct = new TrackingProduct
            {
                Url = addProductRequest.Url,
                AddedByUserId = userId,
                AddedAt = DateTime.UtcNow,
                TrackingProductPrices = new()
                {
                    new()
                    {
                        Price = price.Value,
                        AddedAt = DateTime.UtcNow
                    }
                }
            };

            await dbContext.TrackingProducts.AddAsync(trackingProduct, cancellationToken);
        }

        await dbContext.UserProducts.AddAsync(new UserProduct
        {
            Tag = addProductRequest.Tag,
            UserId = userId,
            TrackingProductId = trackingProduct.TrackingProductId,
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok();
    }
}