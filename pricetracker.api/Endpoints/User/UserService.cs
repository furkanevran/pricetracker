using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PriceTracker.Entities;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<PriceTracker.Entities.User> _userValidator;
    private readonly JwtOptions _jwtOptions;
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public UserService(AppDbContext dbContext,
        IOptions<JwtOptions> jwtOptions,
        IValidator<PriceTracker.Entities.User> userValidator,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PriceTracker.Entities.User?> GetCurrentUserAsync()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return null;

        return await _dbContext.Users.FindAsync(userId);
    }

    public Guid? GetCurrentUserId()
    {
        var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return null;

        return Guid.Parse(userId);
    }

    public async Task<bool> CreateAsync(PriceTracker.Entities.User user, string password)
    {
        var salt = GetRandomSalt();

        user.PasswordSalt = Convert.ToBase64String(salt);
        user.PasswordHash = Convert.ToBase64String(await GetHash(password, salt));

        await _userValidator.ValidateAndThrowAsync(user);
        _dbContext.Users.Add(user);

        return await _dbContext.SaveChangesAsync() == 1;
    }

    private static byte[] GetRandomSalt()
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        return salt;
    }

    private static async Task<byte[]> GetHash(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 4,
            Iterations = 3,
            MemorySize = 1024 * 256
        };

        return await argon2.GetBytesAsync(32);
    }

    private static async Task<bool> VerifyHash(string password, string salt, string hash)
    {
        var newHash = await GetHash(password, Convert.FromBase64String(salt));
        var hashBytes = Convert.FromBase64String(hash);

        return hashBytes.SequenceEqual(newHash);
    }

    private async Task<JwtSecurityToken?> GetVerifiedToken(string token, string secret)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = credentials.Key,
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var validationResult = await tokenHandler.ValidateTokenAsync(token, validationParameters);
            if (validationResult.SecurityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                return jwtSecurityToken;
        }
        catch
        {
            return null;
        }

        return null;
    }

    public string GenerateRefreshToken(Guid userId, int tokenVersion)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new("tokenVersion", tokenVersion.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.RefreshTokenSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpiration),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public (string Token, DateTime ExpiresAt) GenerateAccessToken(string username, Guid userId)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.AccessTokenSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiration),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
    }

    public async Task<TokenResponse> GetTokensFromRefreshToken(string token)
    {
        var verifiedToken = await GetVerifiedToken(token, _jwtOptions.RefreshTokenSecret);
        if (verifiedToken == null)
            throw new Exception("Invalid refresh token.");

        var jti = Guid.Parse(verifiedToken.Id);
        if (await _dbContext.ConsumedRefreshTokens.AnyAsync(x => x.ConsumedRefreshTokenId == jti))
            throw new Exception("Refresh token is blacklisted.");

        var userId = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        var tokenVersion = Convert.ToInt32(verifiedToken.Claims.First(x => x.Type == "tokenVersion").Value);
        if (user.TokenVersion != tokenVersion)
            throw new Exception("Invalid token version");

        var accessToken = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        _dbContext.ConsumedRefreshTokens.Add(new ConsumedRefreshToken
        {
            ConsumedRefreshTokenId = jti,
            ExpiresAt = verifiedToken.ValidTo
        });
        await _dbContext.SaveChangesAsync();

        return new TokenResponse(accessToken.Token, refreshToken, accessToken.ExpiresAt);
    }

    public async Task<TokenResponse> GetTokens(string username, string password)
    {
        var user = _dbContext.Users.First(x => x.Username == username);
        if (!await VerifyHash(password, user.PasswordSalt, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        var token = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        return new TokenResponse(token.Token, refreshToken, token.ExpiresAt);
    }
}