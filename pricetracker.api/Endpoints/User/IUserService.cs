namespace PriceTracker.API.Endpoints.User;

public interface IUserService
{
    Task<PriceTracker.Entities.User?> GetCurrentUserAsync();
    Task<bool> CreateAsync(PriceTracker.Entities.User user, string password);
    string GenerateRefreshToken(Guid userId, int tokenVersion);
    (string Token, DateTime ExpiresAt) GenerateAccessToken(string username, Guid userId);
    Task<TokenResponse> GetTokensFromRefreshToken(string token);
    Task<TokenResponse> GetTokens(string username, string password);
}