using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceTracker.API.Attributes;
using PriceTracker.Entities;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

[Template("/user")]
[Authorize]
public class UserEndpoint : IEndpoint
{
    [HttpPost("get-products")]
    [Authorize]
    public static async Task<GetProductsResponse> GetProducts([FromBody] GetProductsRequest getProductsRequest,
        IAuthService authService,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId()!.Value;

        var products = dbContext.UserProducts
            .Where(x => x.UserId == userId)
            .Include(x => x.TrackingProduct)
            .ThenInclude(x => x.TrackingProductPrices)
            .AsQueryable();

        if (!string.IsNullOrEmpty(getProductsRequest.Tag))
            products = products.Where(x => x.Tag.Contains(getProductsRequest.Tag));

        if (!string.IsNullOrEmpty(getProductsRequest.Url))
            products = products.Where(x => x.TrackingProduct.Url.Contains(getProductsRequest.Url));

        var productsQuery = products.Select(x => new GetProductsResponse.Product
        {
            Price = x.TrackingProduct.TrackingProductPrices!
                .OrderByDescending(x => x.AddedAt)
                .Select(x => x.Price)
                .FirstOrDefault(),
            Url = x.TrackingProduct.Url,
            Tag = x.Tag
        });

        return await productsQuery.GetPaginatedResponse<GetProductsResponse, GetProductsResponse.Product>(getProductsRequest, cancellationToken);
    }
}