namespace Chat.Infrastructure.Abstractions.Auth;

/// <summary>
/// Интерфейс для управления паролями пользователей
/// </summary>
public interface IPasswordManager
{
    /// <summary>
    /// Выполняет хеширование пароля
    /// </summary>
    /// <param name="password">Исходный пароль</param>
    /// <returns>Хешированный пароль в виде строки</returns>
    string HashPassword(string password);
    
    /// <summary>
    /// Проверяет соответствие пароля его хешу
    /// </summary>
    /// <param name="password">Исходный пароль</param>
    /// <param name="hashedPassword">Хешированный пароль для сравнения</param>
    /// <returns>True, если пароль соответствует хешу; иначе - false</returns>
    bool VerifyPassword(string password, string hashedPassword);
}

