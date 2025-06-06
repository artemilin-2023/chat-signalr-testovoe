using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для управления учетными записями пользователей
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Получает информацию о пользователе по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Данные пользователя</returns>
    Task<UserResponse> GetUserAsync(long id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает информацию о текущем авторизованном пользователе
    /// </summary>
    /// <param name="request">HTTP-запрос</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Данные текущего пользователя</returns>
    Task<UserResponse> GetCurrentUserAsync(HttpRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <param name="request">Данные для регистрации</param>
    /// <param name="response">HTTP-ответ для установки заголовков аутентификации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Данные зарегистрированного пользователя</returns>
    Task<UserResponse> RegisterAsync(RegisterRequest request, HttpResponse response, CancellationToken cancellationToken);

    /// <summary>
    /// Выполняет вход пользователя в систему
    /// </summary>
    /// <param name="request">Данные для входа</param>
    /// <param name="response">HTTP-ответ для установки заголовков аутентификации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task LoginAsync(LoginRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// Выполняет выход пользователя из системы
    /// </summary>
    /// <param name="request">HTTP-запрос</param>
    /// <param name="response">HTTP-ответ для удаления заголовков аутентификации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет токен доступа пользователя
    /// </summary>
    /// <param name="request">HTTP-запрос с refresh token</param>
    /// <param name="response">HTTP-ответ для установки нового токена</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}
