using PriceTracker.API.Endpoints.User;
using PriceTracker.Entities;

namespace PriceTracker.API.Services.Auth;

public interface IUserService
{
    Task<User?> GetCurrentUserAsync();
    Task<bool> CreateAsync(User user, string password);
    string GenerateRefreshToken(Guid userId, int tokenVersion);
    (string Token, DateTime ExpiresAt) GenerateAccessToken(string username, Guid userId);
    Task<TokenResponse> GetTokensFromRefreshToken(string token);
    Task<TokenResponse> GetTokens(string username, string password);
}