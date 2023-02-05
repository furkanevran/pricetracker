using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints.User;
using PriceTracker.Entities;
using PriceTracker.Extractor;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.Price;

[Template("/product")]
public class ProductEndpoint : IEndpoint
{
    [HttpPost("add")]
    [Authorize]
    public static async Task<IResult> Add([FromBody] AddProductRequest addProductRequest,
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
                return Results.BadRequest("Could not extract price from url");

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

        var userProduct = await dbContext.UserProducts
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TrackingProductId == trackingProduct.TrackingProductId,
                cancellationToken: cancellationToken);

        await dbContext.UserProducts.AddAsync(new UserProduct
        {
            Tag = addProductRequest.Tag,
            UserId = userId,
            TrackingProductId = trackingProduct.TrackingProductId,
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}