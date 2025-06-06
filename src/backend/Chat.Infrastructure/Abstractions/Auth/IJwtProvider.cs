using System.Security.Claims;

namespace Chat.Infrastructure.Abstractions.Auth;

public interface IJwtProvider
{
    /// <summary>
    /// Генерирует JWT токен для пользователя
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен</param>
    /// <returns>JWT токен в виде строки</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Проверяет JWT токен и возвращает ClaimsPrincipal
    /// </summary>
    /// <param name="token">JWT токен для проверки</param>
    /// <returns>ClaimsPrincipal, если токен действителен</returns>
    IEnumerable<Claim> ValidateTokenAndGetClaims(string token);

    /// <summary>
    /// Генерирует JWT токен для обновления
    /// </summary>
    /// <returns>JWT токен в виде строки</returns>
    string GenerateRefreshToken();
}