using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceTracker.API.Attributes;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

[Tags("User Product")]
[Template("/user/get-products")]
[Authorize]
public class UserGetProductsEndpoint : IEndpoint
{
    public record GetProductsRequest : IPaginatedRequest
    {
        [JsonPropertyName("url")] public string Url { get; set; } = null!;
        [JsonPropertyName("tag"), MaxLength(120)] public string Tag { get; set; } = null!;
        [JsonPropertyName("take")] public int Take { get; set; }
        [JsonPropertyName("skip")] public int Skip { get; set; }
    }

    public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsRequestValidator()
        {
            RuleFor(x => x.Url)
                .MustBeValidHttpsUrl()
                .When(x => !string.IsNullOrEmpty(x.Url));

            RuleFor(x => x.Tag)
                .MaximumLength(120);
        }
    }

    public class GetProductsResponse : IPaginatedResponse<GetProductsResponse.Product>
    {
        public class Product
        {
            public double? Price { get; init; }
            public string Url { get; init; } = null!;
            public string Tag { get; init; } = null!;
        }

        public List<Product> Items { get; set; } = null!;
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
    }

    [HttpPost]
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

        var productsQuery = products.Select(product => new GetProductsResponse.Product
        {
            Price = product.TrackingProduct.TrackingProductPrices!
                .OrderByDescending(productPrice => productPrice.AddedAt)
                .Select(productPrice => productPrice.Price)
                .FirstOrDefault(),
            Url = product.TrackingProduct.Url,
            Tag = product.Tag
        });

        return await productsQuery.GetPaginatedResponse<GetProductsResponse, GetProductsResponse.Product>(getProductsRequest, cancellationToken);
    }
}