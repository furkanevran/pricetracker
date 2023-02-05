namespace PriceTracker.API.Endpoints.User;

public record AccessTokenResponse(string AccessToken, DateTimeOffset ExpiresAt);