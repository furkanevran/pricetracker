using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;
using PriceTracker.Extractor;

namespace PriceTracker.API.Endpoints.Price;

[Template("/price")]
public class PriceEndpoint : IEndpoint
{
    [HttpPost]
    [Authorize]
    public static async Task<double?> Post([FromBody] UrlRequest urlRequest, IExtractor extractor, IHttpContextAccessor httpContextAccessor) =>
        await extractor.ExtractPrice(urlRequest.Url);
}