using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PriceTracker.API.Endpoints.User;
using PriceTracker.Entities;
using PriceTracker.Persistence;

namespace PriceTracker.API.Services.Auth;

public class UserService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;

    public UserService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.NameId);
        if (userId == null)
            return null;

        return await _dbContext.Users.FindAsync(userId);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
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

    public string GenerateAccessToken(string username, Guid userId)
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

        return tokenHandler.WriteToken(token);
    }

    public async Task<TokenResponse> NewTokenFromRefreshToken(string token)
    {
        var verifiedToken = await GetVerifiedToken(token, _jwtOptions.RefreshTokenSecret);
        if (verifiedToken == null)
            throw new Exception("Invalid refresh token.");

        var jti = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
        if (await _dbContext.ConsumedRefreshTokens.AnyAsync(x => x.ConsumedRefreshTokenId == jti))
            throw new Exception("Refresh token is blacklisted.");

        var userId = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        var tokenVersion = Convert.ToInt32(verifiedToken.Claims.First(x => x.Type == "tokenVersion").Value);
        if (user.TokenVersion != tokenVersion)
            throw new Exception("Invalid token version");

        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
        var accessToken = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        _dbContext.ConsumedRefreshTokens.Add(new ConsumedRefreshToken
        {
            ConsumedRefreshTokenId = jti
        });
        await _dbContext.SaveChangesAsync();

        return new TokenResponse(accessToken, refreshToken, tokenExpiresAt);
    }

    public async Task<TokenResponse> GetTokens(LoginRequest request)
    {
        var user = _dbContext.Users.First(x => x.Username == request.Username);
        if (!await VerifyHash(request.Password, user.PasswordSalt, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
        var token = GenerateAccessToken(user.Username, user.UserId);
        var refreshToken = GenerateRefreshToken(user.UserId, user.TokenVersion);

        return new TokenResponse(token, refreshToken, tokenExpiresAt);
    }
}