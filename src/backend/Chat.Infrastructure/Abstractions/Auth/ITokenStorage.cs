namespace Chat.Infrastructure.Abstractions.Auth;

/// <summary>
/// ��������� ��������� ������� �����������
/// </summary>
/// <remarks>
/// ������������� ������ ��� ������ � �������� ���������� (refresh tokens)
/// </remarks>
public interface ITokenStorage
{
    /// <summary>
    /// ������� ����� �� ���������
    /// </summary>
    /// <param name="token">��������� ������������� ������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, �������������� ����������� �������� ��������</returns>
    public Task DeleteTokenAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// �������� ������������� ������������ �� ������
    /// </summary>
    /// <param name="token">��������� ������������� ������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, ���������� ���� ������ �������� � ������������� ������������ (���� ����� ������������)</returns>
    public Task<(bool success, long? id)> GetUserIdAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// ��������� ����� � ���������
    /// </summary>
    /// <param name="token">��������� ������������� ������</param>
    /// <param name="userId">������������� ������������, �������� ����������� �����</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, �������������� ����������� �������� ����������</returns>
    public Task SetTokenAsync(string token, long userId, CancellationToken cancellationToken);
}