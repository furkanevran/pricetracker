using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints.User;
using PriceTracker.Entities;
using PriceTracker.Extractor;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.Price;

[Template("/price")]
public class PriceEndpoint : IEndpoint
{
    [HttpPost]
    [Authorize]
    public static async Task<double?> Post([FromBody] UrlRequest urlRequest, IExtractor extractor, IUserService userService, AppDbContext dbContext)
    {
        var userId = userService.GetCurrentUserId()!.Value;

        var trackingProduct = await dbContext.TrackingProducts
            .FirstOrDefaultAsync(x => x.Url == urlRequest.Url);

        if (trackingProduct != null)
            return
                (await dbContext.TrackingProductPrices.
                    OrderByDescending(x => x.AddedAt).
                    FirstAsync(x => x.TrackingProductId == trackingProduct.TrackingProductId))
                .Price;

        var price = await extractor.ExtractPrice(urlRequest.Url);
        if (price == null)
            return null;

        trackingProduct = new TrackingProduct
        {
            Url = urlRequest.Url,
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

        dbContext.TrackingProducts.Add(trackingProduct);
        await dbContext.SaveChangesAsync();

        return price;
    }
}