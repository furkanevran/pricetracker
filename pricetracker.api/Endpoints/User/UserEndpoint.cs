using Microsoft.AspNetCore.Mvc;
using PriceTracker.API.Attributes;

namespace PriceTracker.API.Endpoints.User;

[Template("/user")]
public class UserEndpoint : IEndpoint
{
    [HttpPost("register")]
    public static async Task<bool> Register(RegisterRequest request, IUserService userService)
    {
        var user = new PriceTracker.Entities.User
        {
            UserId = Guid.NewGuid(),
            EMail = request.EMail,
            Username = request.Username,
            TokenVersion = 1
        };

        return await userService.CreateAsync(user, request.Password);
    }

    private static void SetRefreshTokenCookie(IHttpContextAccessor contextAccessor, Token tokens)
    {
        contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = tokens.RefreshTokenExpiresAt
        });
    }

    [HttpPost("login")]
    public static async Task<AccessTokenResponse> Login(LoginRequest request, IUserService userService, IHttpContextAccessor contextAccessor)
    {
        var tokens = await userService.GetTokens(request.Username, request.Password);

        SetRefreshTokenCookie(contextAccessor, tokens);

        return new AccessTokenResponse(tokens.AccessToken, tokens.AccessTokenExpiresAt);
    }

    [HttpPost("newtoken")]
    public static async Task<AccessTokenResponse> NewTokenFromRefreshToken(IUserService userService, IHttpContextAccessor contextAccessor)
    {
        contextAccessor.HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            throw new Exception("No refresh token found");

        var tokens = await userService.GetTokensFromRefreshToken(refreshToken);

        SetRefreshTokenCookie(contextAccessor, tokens);

        return new AccessTokenResponse(tokens.AccessToken, tokens.AccessTokenExpiresAt);
    }

}