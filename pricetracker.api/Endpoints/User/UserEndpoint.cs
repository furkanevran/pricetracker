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

    [HttpPost("login")]
    public static async Task<TokenResponse> Login(LoginRequest request, IUserService userService)
    {
        return await userService.GetTokens(request.Username, request.Password);
    }

    [HttpPost("newtoken")]
    public static async Task<TokenResponse> NewTokenFromRefreshToken([FromBody] string token, IUserService userService)
    {
        return await userService.GetTokensFromRefreshToken(token);
    }
}