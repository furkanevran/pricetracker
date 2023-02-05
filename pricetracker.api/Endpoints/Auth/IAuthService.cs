namespace PriceTracker.API.Endpoints.User;

public interface IAuthService
{
    Task<PriceTracker.Entities.User?> GetCurrentUserAsync();
    Guid? GetCurrentUserId();
    Task<bool> CreateAsync(PriceTracker.Entities.User user, string password, CancellationToken cancellationToken = default);
    (string Token, DateTimeOffset ExpiresAt) GenerateRefreshToken(Guid userId, int tokenVersion);
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(string username, Guid userId);
    Task<Token> GetTokensFromRefreshToken(string token, CancellationToken cancellationToken = default);
    Task<Token> GetTokens(string username, string password, CancellationToken cancellationToken = default);
}