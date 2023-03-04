using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceTracker.API.Attributes;
using OneOf;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

[Tags("User Product")]
[Template("/user/remove-product")]
[Authorize]
public class UserRemoveProductEndpoint : IEndpoint
{
    public record RemoveProductRequest(string Url);

    public class RemoveProductRequestValidator : AbstractValidator<RemoveProductRequest>
    {
        public RemoveProductRequestValidator()
        {
            RuleFor(x => x.Url)
                .MustBeValidHttpsUrl()
                .When(x => !string.IsNullOrEmpty(x.Url));
        }
    }

    [HttpPost]
    public static async Task<OneOf<Ok, NotFound>> Remove([FromBody] RemoveProductRequest removeProductRequest,
        IAuthService authService,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId()!.Value;

        var userProduct = await dbContext.UserProducts
            .Include(x => x.TrackingProduct)
            .FirstOrDefaultAsync(x => x.TrackingProduct.Url == removeProductRequest.Url && x.UserId == userId, cancellationToken: cancellationToken);

        if (userProduct == null)
            return TypedResults.NotFound();

        dbContext.UserProducts.Remove(userProduct);

        await dbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok();
    }
}