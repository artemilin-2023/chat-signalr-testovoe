using Chat.Infrastructure.Abstractions.Auth;
using Chat.Infrastructure.ServiceRegistration.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Infrastructure.Auth
{
    public class JwtProvider(ILogger<JwtProvider> logger, IOptions<JwtOptions> options) :
        IJwtProvider
    {
        private readonly ILogger<JwtProvider> logger = logger;
        private readonly JwtOptions options = options.Value;

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = GetTokenDescriptor(
                    options.Secret,
                    options.AccessTokenExpirationHours,
                    [.. claims]
                );

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при генерации JWT токена");
                throw;
            }
        }

        private static SecurityTokenDescriptor GetTokenDescriptor(string key, int expirationHours, params Claim[]? claims)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }

        public string GenerateRefreshToken()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = GetTokenDescriptor(
                    options.Secret,
                    options.RefreshTokenExpirationHours
                );

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при генерации JWT токена");
                throw;
            }
        }

        public IEnumerable<Claim> ValidateTokenAndGetClaims(string token)
        {
            ArgumentNullException.ThrowIfNull(token);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(options.Secret);
                var validationOptions = options.TokenValidation;

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = validationOptions.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = validationOptions.ValidateIssuer,
                    ValidateAudience = validationOptions.ValidateAudience,
                    ClockSkew = validationOptions.ClockSkew
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal.Claims;
            }
            catch (SecurityTokenExpiredException)
            {
                throw;
            }
            catch (SecurityTokenException ex)
            {
                logger.LogWarning(ex, "Недействительный токен");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при валидации токена");
                throw;
            }
        }
    }
}

