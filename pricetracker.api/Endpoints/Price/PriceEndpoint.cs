using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;
using PriceTracker.Extractor;

namespace PriceTracker.API.Endpoints.Price;

[Pattern("/price")]
public class PriceEndpoint : IEndpoint
{
    [HttpPost]
    public static async Task<double?> Post ([FromBody] UrlRequest urlRequest, IExtractor extractor) => await extractor.ExtractPrice(urlRequest.Url);
    
    [HttpDelete]
    public static string Delete (HttpContext ctx) => "Delete" + ctx.Request.Path;
    
    [HttpPut]
    public static string Put (HttpContext ctx) => "Put" + ctx.Request.Path;
    
    [HttpPatch]
    public static string Patch (HttpContext ctx) => "Patch" + ctx.Request.Path;
    
    [HttpGet("{id}")]
    public static string Get (HttpContext ctx, int id) => "Get " + id + " " + ctx.Request.Path;
    
    [HttpHead]
    public static string Head (HttpContext ctx) => "Head" + ctx.Request.Path;
    
    [HttpOptions]
    public static void Options (HttpContext ctx)
    {
        ctx.Response.Headers.Add("test", "test");
    }
}