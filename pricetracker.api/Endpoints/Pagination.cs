using Microsoft.EntityFrameworkCore;

namespace PriceTracker.API.Endpoints;

public interface IPaginatedRequest
{
    int Take { get; set; }
    int Skip { get; set; }
}

public interface IPaginatedResponse<T>
{
    List<T> Items { get; set; }
    int Page { get; set; }
    int PageSize { get; set; }
    int TotalPages { get; set; }
    int TotalItems { get; set; }
}

// public class PaginatedResponse<T> : IPaginatedResponse<T>
// {
//     public List<T> Items { get; set; }
//     public int Page { get; set; }
//     public int PageSize { get; set; }
//     public int TotalPages { get; set; }
//     public int TotalItems { get; set; }
// }

public static class Pagination
{
    public static async Task<T> GetPaginatedResponse<T, U>(this IQueryable<U> query, IPaginatedRequest request, CancellationToken cancellationToken = default)
        where T : class, IPaginatedResponse<U>, new()
        where U : class
    {

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalItems / request.Take);

        var items = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);

        return new T
        {
            Items = items,
            Page = request.Skip / request.Take + 1,
            PageSize = request.Take,
            TotalPages = totalPages,
            TotalItems = totalItems
        };
    }

    // public static async Task<IPaginatedResponse<T>> GetPaginatedResponse<T>(this IQueryable<T> query, IPaginatedRequest request, CancellationToken cancellationToken)
    // {
    //     var totalItems = await query.CountAsync(cancellationToken);
    //     var totalPages = (int)Math.Ceiling((double)totalItems / request.Take);
    //     var items = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
    //
    //     return new PaginatedResponse<T>
    //     {
    //         Items = items,
    //         Page = request.Skip / request.Take + 1,
    //         PageSize = request.Take,
    //         TotalPages = totalPages,
    //         TotalItems = totalItems
    //     };
    // }
}