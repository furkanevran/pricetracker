using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceTracker.API.Endpoints.User;

public class GetProductsResponse : IPaginatedResponse
{
    public record Product(double? Price, string Url, string Tag);
    public IEnumerable<Product> Products { get; set; } = null!;

    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}