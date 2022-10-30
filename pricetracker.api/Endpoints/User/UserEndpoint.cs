using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceTracker.API.Attributes;
using PriceTracker.Entities;
using PriceTracker.Persistence;

namespace PriceTracker.API.Endpoints.User;

[Template("/user")]
public class UserEndpoint : IEndpoint
{
    [HttpPost("register")]
    public static async Task<bool> Register(RegisterRequest request, AppDbContext dbContext, IValidator<PriceTracker.Entities.User> userValidator)
    {
        var user = new PriceTracker.Entities.User
        {
            UserId = Guid.NewGuid(),
            EMail = request.EMail,
            Username = request.Username,
            TokenVersion = 1
        };
        var salt = GetRandomSalt();

        user.PasswordSalt = Convert.ToBase64String(salt);
        user.PasswordHash = Convert.ToBase64String(await GetHash(request.Password, salt));

        await userValidator.ValidateAndThrowAsync(user);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }

    [HttpPost("login")]
    public static async Task<TokenResponse> Login(LoginRequest request, AppDbContext dbContext)
    {
        var user = dbContext.Users.First(x => x.Username == request.Username);
        if (!await VerifyHash(request.Password, user.PasswordSalt, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
        var token = GenerateAccessToken(user.Username, user.UserId, tokenExpiresAt);
        var refreshToken = GenerateRefreshToken(user.UserId, tokenExpiresAt.AddMonths(1), user.TokenVersion);

        return new TokenResponse(token, refreshToken, tokenExpiresAt);
    }

    [HttpPost("newtoken")]
    public static async Task<TokenResponse> NewTokenFromRefreshToken([FromBody] string token, AppDbContext dbContext)
    {
        var verifiedToken = await VerifyToken(token, "1This is a very long secret key that should be stored in a secure place");
        if (verifiedToken == null)
            throw new Exception("Invalid refresh token.");

        var jti = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
        if (await dbContext.ConsumedRefreshTokens.AnyAsync(x => x.ConsumedRefreshTokenId == jti))
            throw new Exception("Refresh token is blacklisted.");

        var userId = Guid.Parse(verifiedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
        var user = dbContext.Users.First(x => x.UserId == userId);

        var tokenVersion = Convert.ToInt32(verifiedToken.Claims.First(x => x.Type == "tokenVersion").Value);
        if (user.TokenVersion != tokenVersion)
            throw new Exception("Invalid token version");

        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
        var accessToken = GenerateAccessToken(user.Username, user.UserId, tokenExpiresAt);
        var refreshToken = GenerateRefreshToken(user.UserId, tokenExpiresAt.AddMonths(1), user.TokenVersion);

        dbContext.ConsumedRefreshTokens.Add(new ConsumedRefreshToken
        {
            ConsumedRefreshTokenId = jti
        });
        await dbContext.SaveChangesAsync();

        return new TokenResponse(accessToken, refreshToken, tokenExpiresAt);
    }

    // [HttpPost]
    // public static async Task<PriceTracker.Entities.User> Me(string token)
    // {
    //     var userClaims = GetClaimsPrincipal(token)?.Claims;
    //     if (userClaims == null)
    //     {
    //         throw new Exception("Invalid token");
    //     }
    //
    //     var user = _users.First(x => x.Id == new UserId(Guid.Parse((userClaims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value))));
    //     return await Task.FromResult(user);
    // }

    public static byte[] GetRandomSalt()
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        return salt;
    }

    public static async Task<byte[]> GetHash(string password, byte[] salt)
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
    
    public static async Task<bool> VerifyHash(string password, string salt, string hash)
    {
        var newHash = await GetHash(password, Convert.FromBase64String(salt));
        var hashBytes = Convert.FromBase64String(hash);
        
        return hashBytes.SequenceEqual(newHash);
    }

    public static string GenerateAccessToken(string username, Guid userId, DateTimeOffset validUntil)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is a very long secret key that should be stored in a secure place"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = validUntil.UtcDateTime,
            SigningCredentials = credentials,
            Issuer = "http://localhost:7286",
            Audience = "http://localhost:7286",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static string GenerateRefreshToken(Guid userId, DateTimeOffset validUntil, int tokenVersion)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new("tokenVersion", tokenVersion.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1This is a very long secret key that should be stored in a secure place"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = validUntil.UtcDateTime,
            SigningCredentials = credentials,
            Issuer = "http://localhost:7286",
            Audience = "http://localhost:7286",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    public static async Task<JwtSecurityToken?> VerifyToken(string token, string secret)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = credentials.Key,
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:7286",
            ValidateAudience = true,
            ValidAudience = "http://localhost:7286",
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
    
    public static ClaimsPrincipal? GetClaimsPrincipal(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is a very long secret key that should be stored in a secure place"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = credentials.Key,
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:7286",
            ValidateAudience = true,
            ValidAudience = "http://localhost:7286",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }
    
    public static bool IsTokenValid(string token)
    {
        var claimsPrincipal = GetClaimsPrincipal(token);
        if (claimsPrincipal == null)
            return false;

        var expiration = DateTime.Parse(claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Expiration).Value);
        var tokenVersion = int.Parse(claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Version).Value);
        var userId = Guid.Parse(claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        return false;
    }
}