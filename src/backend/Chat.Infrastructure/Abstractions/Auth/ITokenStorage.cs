namespace Chat.Infrastructure.Abstractions.Auth;

/// <summary>
/// Интерфейс хранилища токенов авторизации
/// </summary>
/// <remarks>
/// Предоставляет методы для работы с токенами обновления (refresh tokens)
/// </remarks>
public interface ITokenStorage
{
    /// <summary>
    /// Удаляет токен из хранилища
    /// </summary>
    /// <param name="token">Строковое представление токена</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления</returns>
    public Task DeleteTokenAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает идентификатор пользователя по токену
    /// </summary>
    /// <param name="token">Строковое представление токена</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Кортеж, содержащий флаг успеха операции и идентификатор пользователя (если токен действителен)</returns>
    public Task<(bool success, long? id)> GetUserIdAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// Сохраняет токен в хранилище
    /// </summary>
    /// <param name="token">Строковое представление токена</param>
    /// <param name="userId">Идентификатор пользователя, которому принадлежит токен</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, представляющая асинхронную операцию сохранения</returns>
    public Task SetTokenAsync(string token, long userId, CancellationToken cancellationToken);
}