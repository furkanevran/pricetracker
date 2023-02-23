namespace PriceTracker.API.Endpoints.User;

public record TokenResponse(string AccessToken, DateTimeOffset AccessTokenExpiresAt, DateTimeOffset RefreshTokenExpiresAt);