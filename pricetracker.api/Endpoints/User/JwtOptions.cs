namespace PriceTracker.API.Endpoints.User;

public class JwtOptions
{
    public const string SectionKey = "Jwt";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string AccessTokenSecret { get; set; } = null!;
    public string RefreshTokenSecret { get; set; } = null!;
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
}