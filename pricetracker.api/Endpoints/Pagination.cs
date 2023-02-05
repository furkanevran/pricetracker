namespace PriceTracker.API.Endpoints;

public interface IPaginatedRequest
{
    int Take { get; set; }
    int Skip { get; set; }
}

public interface IPaginatedResponse
{
    int Page { get; set; }
    int PageSize { get; set; }
    int TotalPages { get; set; }
    int TotalItems { get; set; }
}