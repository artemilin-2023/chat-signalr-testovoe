using Chat.Application.Abstractions.Services;
using Chat.Application.Constants;
using Chat.Contracts.Exceptions;
using Chat.Domain;
using Chat.Infrastructure.Abstractions.Auth;
using Chat.Infrastructure.Abstractions.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chat.Application.Services;

/// <inheritdoc/>
public class AuthService :
    IAuthService
{
    private const string BearerPrefix = "Bearer ";
    public const string AuthorizationHeader = "Authorization";
    public const string RefreshTokenHeader = "Refresh-Token";

    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRepository<User> _userRepository;

    public AuthService(IJwtProvider jwtProvider, ITokenStorage tokenStorage, IRepository<User> userRepository)
    {
        _jwtProvider = jwtProvider;
        _tokenStorage = tokenStorage;
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new (CustomClaimTypes.UserId, user.Id.ToString())
        };

        return _jwtProvider.GenerateAccessToken(claims);
    }

    /// <inheritdoc/>
    public async Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var token = _jwtProvider.GenerateRefreshToken();
        await _tokenStorage.SetTokenAsync(token, user.Id, cancellationToken);
        return token;
    }

    /// <inheritdoc/>
    public async Task<(string accessToken, string refreshToken)> GenerateAccessRefreshPairAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user, cancellationToken);

        return (accessToken, refreshToken);
    }

    /// <inheritdoc/>
    public async Task GenerateAndSetTokensAsync(User user, HttpResponse response, CancellationToken cancellationToken)
    {
        var pair = await GenerateAccessRefreshPairAsync(user, cancellationToken);
        
        var (accessToken, refreshToken) = pair;

        SetTokenToHeader(response, accessToken, AuthorizationHeader);
        SetTokenToHeader(response, refreshToken, RefreshTokenHeader);
    }

    /// <inheritdoc/>
    public static void SetTokenToHeader(HttpResponse response, string token, string header = AuthorizationHeader)
    {
        if (header != AuthorizationHeader && header != RefreshTokenHeader)
            throw UnauthorizedException.InvalidHeader();

        if (header == AuthorizationHeader)
            token = BearerPrefix + token;

        response.Headers[header] = token;
    }

    /// <inheritdoc/>
    public long GetUserIdFromHttpRequest(HttpRequest request)
    {
        var token = GetTokenFromHeader(request);

        var userId = GetClaimFromToken(token, CustomClaimTypes.UserId,
            value => long.TryParse(value, out var id)
                ? id
                : throw UnauthorizedException.InvalidToken()
        );

        return userId;
    }

    private static string GetTokenFromHeader(HttpRequest request, string header = AuthorizationHeader)
    {
        if (!request.Headers.TryGetValue(header, out var token) || string.IsNullOrEmpty(token))
            throw UnauthorizedException.InvalidToken();

        if (header == AuthorizationHeader)
            return token.ToString().Replace(BearerPrefix, "");

        return token.ToString();
    }

    private T GetClaimFromToken<T>(string token, string claimType, Func<string, T> parseFunc)
    {
        var claims = _jwtProvider.ValidateTokenAndGetClaims(token);

        var claim = claims.FirstOrDefault(c => c.Type == claimType)
            ?? throw new Exception($"Failed to get claim {claimType}");

        return parseFunc(claim.Value);
    }

    /// <inheritdoc/>
    public async Task ClearTokensAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        response.Headers.Remove(AuthorizationHeader);

        if (request.Headers.TryGetValue(RefreshTokenHeader, out var refreshToken) && !string.IsNullOrEmpty(refreshToken))
        {
            response.Headers.Remove(RefreshTokenHeader);

            await _tokenStorage.DeleteTokenAsync(refreshToken!, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader(request, RefreshTokenHeader);
        var (success, userId) = await _tokenStorage.GetUserIdAsync(token, cancellationToken);
        if (success is false)
            throw UnauthorizedException.TokenExpaired();

        var user = await _userRepository
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw NotFoundException.UserNotFound();
        
        var accessToken = GenerateAccessToken(user);

        SetTokenToHeader(response, accessToken, AuthorizationHeader);
    }
}