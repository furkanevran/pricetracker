namespace PriceTracker.API.Endpoints.User;

public interface IUserService
{
    Task<PriceTracker.Entities.User?> GetCurrentUserAsync();
    Guid? GetCurrentUserId();
    Task<bool> CreateAsync(PriceTracker.Entities.User user, string password);
    (string Token, DateTimeOffset ExpiresAt) GenerateRefreshToken(Guid userId, int tokenVersion);
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(string username, Guid userId);
    Task<Token> GetTokensFromRefreshToken(string token);
    Task<Token> GetTokens(string username, string password);
}