namespace PriceTracker.API.Endpoints.User;


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

