using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PriceTracker.API.Attributes;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.Price;

[Tags("Product")]
[Template("/product/get")]
public class ProductGetEndpoint : IEndpoint
{
    public record GetResponse(string Url, double? Price);
    public record GetProductRequest(string Url);
    public class GetProductRequestValidator : AbstractValidator<GetProductRequest>
    {
        public GetProductRequestValidator()
        {
            RuleFor(x => x.Url)
                .MustBeValidHttpsUrl()
                .When(x => !string.IsNullOrEmpty(x.Url));
        }
    }

    private static async Task<OneOf<GetResponse, NotFound>> GetProduct(GetProductRequest getProductRequest, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var trackingProduct = await dbContext.TrackingProducts
            .Include(x => x.TrackingProductPrices)
            .OrderByDescending(x => x.TrackingProductPrices!.Max(y => y.AddedAt))
            .FirstOrDefaultAsync(x => x.Url == getProductRequest.Url, cancellationToken: cancellationToken);

        if (trackingProduct?.TrackingProductPrices is not {Count: > 0})
            return TypedResults.NotFound();

        var price = trackingProduct.TrackingProductPrices[0].Price;
        return new GetResponse(getProductRequest.Url, price);
    }

    [HttpPost]
    public static async Task<OneOf<GetResponse, NotFound>> Post([FromBody] GetProductRequest getProductRequest,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await GetProduct(getProductRequest, dbContext, cancellationToken);
    }

    [HttpGet("{url}")]
    public static async Task<OneOf<GetResponse, NotFound>> Get([FromQuery] string url,
        IValidator<GetProductRequest> validator,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var request = new GetProductRequest(url);
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        return await GetProduct(request, dbContext, cancellationToken);
    }
}