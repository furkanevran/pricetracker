namespace PriceTracker.API.Services.Auth;

public class JwtOptions
{
    public const string SectionKey = "Jwt";

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string AccessTokenSecret { get; set; }
    public string RefreshTokenSecret { get; set; }
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
}