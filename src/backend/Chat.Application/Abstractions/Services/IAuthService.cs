using Chat.Domain;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Abstractions.Services;

/// <summary>
/// ��������� ������� ��� ������ � ��������������� � ������������
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// ���������� JWT ����� ������� ��� ������������
    /// </summary>
    /// <param name="user">������������, ��� �������� ������������ �����</param>
    /// <returns>������ � JWT ������� �������</returns>
    string GenerateAccessToken(User user);
    
    /// <summary>
    /// ���������� refresh ����� ��� ������������
    /// </summary>
    /// <param name="user">������������, ��� �������� ������������ �����</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������ � refresh �������</returns>
    Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken);
    
    /// <summary>
    /// ���������� ���� ������� (������� � ����������) ��� ������������
    /// </summary>
    /// <param name="user">������������, ��� �������� ������������ ������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������ �� ����� access � refresh �������</returns>
    Task<(string accessToken, string refreshToken)> GenerateAccessRefreshPairAsync(User user, CancellationToken cancellationToken);
    
    /// <summary>
    /// ���������� ������ � ������������� �� � HTTP-�����
    /// </summary>
    /// <param name="user">������������, ��� �������� ������������ ������</param>
    /// <param name="response">HTTP-�����, � ������� ����� ����������� ���� � ��������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    Task GenerateAndSetTokensAsync(User user, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// �������� ������������� ������������ �� HTTP-�������
    /// </summary>
    /// <param name="request">HTTP-������, ���������� ����� �����������</param>
    /// <returns>������������� ������������</returns>
    long GetUserIdFromHttpRequest(HttpRequest request);
    
    /// <summary>
    /// ������� ������ �����������
    /// </summary>
    /// <param name="request">HTTP-������ � �������� ��������</param>
    /// <param name="response">HTTP-����� ��� �������� ���� � ��������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    Task ClearTokensAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// ��������� ����� ������� � �������������� refresh ������
    /// </summary>
    /// <param name="request">HTTP-������, ���������� refresh �����</param>
    /// <param name="response">HTTP-����� ��� ��������� ����� �������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    Task RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}

