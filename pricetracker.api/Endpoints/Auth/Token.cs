namespace PriceTracker.API.Endpoints.User;

public record Token(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiresAt, DateTimeOffset RefreshTokenExpiresAt);