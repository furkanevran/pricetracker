using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;

namespace PriceTracker.API.Endpoints.User;

[Template("/auth")]
public class AuthEndpoint : IEndpoint
{
    [HttpPost("register")]
    public static async Task<IResult> Register(RegisterRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var user = new PriceTracker.Entities.User
        {
            UserId = Guid.NewGuid(),
            EMail = request.EMail,
            Username = request.Username,
            TokenVersion = 1
        };

        await authService.CreateAsync(user, request.Password, cancellationToken);
        return Results.Ok();
    }

    private static void SetRefreshTokenCookie(IHttpContextAccessor contextAccessor, Token tokens)
    {
        if (contextAccessor == null) throw new ArgumentNullException(nameof(contextAccessor));
        if (contextAccessor.HttpContext == null) throw new ArgumentNullException(nameof(contextAccessor));
        if (tokens == null) throw new ArgumentNullException(nameof(tokens));

        contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = tokens.RefreshTokenExpiresAt
        });
    }

    [HttpPost("login")]
    public static async Task<AccessTokenResponse> Login(LoginRequest request,
        IAuthService authService,
        IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken)
    {
        var tokens = await authService.GetTokens(request.Username, request.Password, cancellationToken);

        SetRefreshTokenCookie(contextAccessor, tokens);

        return new AccessTokenResponse(tokens.AccessToken, tokens.AccessTokenExpiresAt);
    }

    [HttpPost("new-token")]
    public static async Task<AccessTokenResponse> NewTokenFromRefreshToken(IAuthService authService,
        IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken)
    {
        if (contextAccessor == null) throw new ArgumentNullException(nameof(contextAccessor));
        if (contextAccessor.HttpContext == null) throw new ArgumentNullException(nameof(contextAccessor));

        contextAccessor.HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            throw new Exception("No refresh token found");

        var tokens = await authService.GetTokensFromRefreshToken(refreshToken, cancellationToken);

        SetRefreshTokenCookie(contextAccessor, tokens);

        return new AccessTokenResponse(tokens.AccessToken, tokens.AccessTokenExpiresAt);
    }

}