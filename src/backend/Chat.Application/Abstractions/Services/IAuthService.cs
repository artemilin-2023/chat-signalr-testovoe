using Chat.Domain;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с аутентификацией и авторизацией
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Генерирует JWT токен доступа для пользователя
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен</param>
    /// <returns>Строка с JWT токеном доступа</returns>
    string GenerateAccessToken(User user);
    
    /// <summary>
    /// Генерирует refresh токен для пользователя
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Строка с refresh токеном</returns>
    Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken);
    
    /// <summary>
    /// Генерирует пару токенов (доступа и обновления) для пользователя
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируются токены</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Кортеж из строк access и refresh токенов</returns>
    Task<(string accessToken, string refreshToken)> GenerateAccessRefreshPairAsync(User user, CancellationToken cancellationToken);
    
    /// <summary>
    /// Генерирует токены и устанавливает их в HTTP-ответ
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируются токены</param>
    /// <param name="response">HTTP-ответ, в который будут установлены куки с токенами</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task GenerateAndSetTokensAsync(User user, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает идентификатор пользователя из HTTP-запроса
    /// </summary>
    /// <param name="request">HTTP-запрос, содержащий токен авторизации</param>
    /// <returns>Идентификатор пользователя</returns>
    long GetUserIdFromHttpRequest(HttpRequest request);
    
    /// <summary>
    /// Очищает токены авторизации
    /// </summary>
    /// <param name="request">HTTP-запрос с текущими токенами</param>
    /// <param name="response">HTTP-ответ для удаления куки с токенами</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task ClearTokensAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет токен доступа с использованием refresh токена
    /// </summary>
    /// <param name="request">HTTP-запрос, содержащий refresh токен</param>
    /// <param name="response">HTTP-ответ для установки новых токенов</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}

