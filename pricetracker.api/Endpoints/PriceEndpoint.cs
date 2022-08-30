using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;
using PriceTracker.Extractor;

namespace PriceTracker.API.Endpoints;

[Pattern("/price")]
public class PriceEndpoint : IEndpoint
{
    public Delegate Post =>
        async ([FromBody] UrlRequest urlRequest, IExtractor extractor) => await extractor.ExtractPrice(urlRequest.Url);
}