namespace PriceTracker.API.Endpoints.User;

public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);