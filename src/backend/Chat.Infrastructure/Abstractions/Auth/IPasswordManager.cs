namespace Chat.Infrastructure.Abstractions.Auth;

/// <summary>
/// ��������� ��� ���������� �������� �������������
/// </summary>
public interface IPasswordManager
{
    /// <summary>
    /// ��������� ����������� ������
    /// </summary>
    /// <param name="password">�������� ������</param>
    /// <returns>������������ ������ � ���� ������</returns>
    string HashPassword(string password);
    
    /// <summary>
    /// ��������� ������������ ������ ��� ����
    /// </summary>
    /// <param name="password">�������� ������</param>
    /// <param name="hashedPassword">������������ ������ ��� ���������</param>
    /// <returns>True, ���� ������ ������������� ����; ����� - false</returns>
    bool VerifyPassword(string password, string hashedPassword);
}

