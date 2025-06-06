using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Abstractions.Services;

/// <summary>
/// ��������� ������� ��� ���������� �������� �������� �������������
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// �������� ���������� � ������������ �� ��������������
    /// </summary>
    /// <param name="id">������������� ������������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������ ������������</returns>
    Task<UserResponse> GetUserAsync(long id, CancellationToken cancellationToken);
    
    /// <summary>
    /// �������� ���������� � ������� �������������� ������������
    /// </summary>
    /// <param name="request">HTTP-������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������ �������� ������������</returns>
    Task<UserResponse> GetCurrentUserAsync(HttpRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// ������������ ������ ������������
    /// </summary>
    /// <param name="request">������ ��� �����������</param>
    /// <param name="response">HTTP-����� ��� ��������� ���������� ��������������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������ ������������������� ������������</returns>
    Task<UserResponse> RegisterAsync(RegisterRequest request, HttpResponse response, CancellationToken cancellationToken);

    /// <summary>
    /// ��������� ���� ������������ � �������
    /// </summary>
    /// <param name="request">������ ��� �����</param>
    /// <param name="response">HTTP-����� ��� ��������� ���������� ��������������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, �������������� ����������� ��������</returns>
    Task LoginAsync(LoginRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// ��������� ����� ������������ �� �������
    /// </summary>
    /// <param name="request">HTTP-������</param>
    /// <param name="response">HTTP-����� ��� �������� ���������� ��������������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, �������������� ����������� ��������</returns>
    Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
    
    /// <summary>
    /// ��������� ����� ������� ������������
    /// </summary>
    /// <param name="request">HTTP-������ � refresh token</param>
    /// <param name="response">HTTP-����� ��� ��������� ������ ������</param>
    /// <param name="cancellationToken">����� ������ ��������</param>
    /// <returns>������, �������������� ����������� ��������</returns>
    Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}
