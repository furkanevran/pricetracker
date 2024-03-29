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

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AuthService> _logger;
    private readonly IValidator<PriceTracker.Entities.User> _userValidator;
    private readonly JwtOptions _jwtOptions;
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AuthService(AppDbContext dbContext,
        ILogger<AuthService> logger,
        IOptions<JwtOptions> jwtOptions,
        IValidator<PriceTracker.Entities.User> userValidator,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<bool> CreateAsync(PriceTracker.Entities.User user, string password, CancellationToken cancellationToken = default)
    {
        var salt = GetRandomSalt();

        user.PasswordSalt = Convert.ToBase64String(salt);
        user.PasswordHash = Convert.ToBase64String(await GetHash(password, salt));

        await _userValidator.ValidateAndThrowAsync(user, cancellationToken: cancellationToken);
        _dbContext.Users.Add(user);

        try
        {
            var resp = await _dbContext.SaveChangesAsync(cancellationToken) == 1;
            _logger.LogInformation("Created user {Username}", user.Username);
            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create user");
            return false;
        }
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
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to validate token");
            return null;
        }

        _logger.LogWarning("Failed to validate token");

        return null;
    }

    public (string Token, DateTimeOffset ExpiresAt) GenerateRefreshToken(Guid userId, int tokenVersion)
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

        _logger.LogInformation("Generated refresh token for user {UserId}", userId);

        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
    }

    public (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(string username, Guid userId)
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

        _logger.LogInformation("Generated access token for user {Username}", username);

        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
    }

    public async Task<Token?> GetTokensFromRefreshToken(string token, CancellationToken cancellationToken = default)
    {
        var verifiedToken = await GetVerifiedToken(token, _jwtOptions.RefreshTokenSecret);
        if (verifiedToken == null)
            return null;

        var jti = Guid.Parse(verifiedToken.Id);
        if (await _dbContext.ConsumedRefreshTokens.AnyAsync(x => x.ConsumedRefreshTokenId == jti, cancellationToken: cancellationToken))
        {
            _logger.LogInformation("Refresh token {Jti} has already been consumed", jti);
            return null;
        }

        var userId = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
        var user = await _dbContext.Users.FindAsync(new object?[] { userId }, cancellationToken: cancellationToken);

        if (user == null)
        {
            _logger.LogInformation("User with id {UserId} not found", userId);
            return null;
        }

        var tokenVersion = Convert.ToInt32(verifiedToken.Claims.First(x => x.Type == "tokenVersion").Value);
        if (user.TokenVersion != tokenVersion)
        {
            _logger.LogInformation("Token version {TokenVersion} for user {UserId} is invalid", tokenVersion, userId);
            return null;
        }

        var accessToken = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        _dbContext.ConsumedRefreshTokens.Add(new ConsumedRefreshToken
        {
            ConsumedRefreshTokenId = jti,
            ExpiresAt = verifiedToken.ValidTo
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Refresh token {RefreshToken} consumed", token);

        return new Token(accessToken.Token, refreshToken.Token, accessToken.ExpiresAt, refreshToken.ExpiresAt);
    }

    public async Task<Token?> GetTokens(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken: cancellationToken);
        if (user == null)
        {
            _logger.LogInformation("User {Username} not found", username);
            return null;
        }

        if (!await VerifyHash(password, user.PasswordSalt, user.PasswordHash))
        {
            _logger.LogInformation("Invalid password for user {Username}", username);
            return null;
        }

        var token = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        return new Token(token.Token, refreshToken.Token, token.ExpiresAt, refreshToken.ExpiresAt);
    }
}