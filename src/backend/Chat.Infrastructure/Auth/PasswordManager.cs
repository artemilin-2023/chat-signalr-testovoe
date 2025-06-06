using Chat.Infrastructure.Abstractions.Auth;

namespace Chat.Infrastructure.Auth;

public class PasswordManager :
    IPasswordManager
{

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));

        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}