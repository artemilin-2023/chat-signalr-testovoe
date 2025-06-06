using Chat.Application.Abstractions.Services;
using Chat.Application.Mappers;
using Chat.Contracts.ApiContracts;
using Chat.Contracts.Exceptions;
using Chat.Domain;
using Chat.Infrastructure.Abstractions.Auth;
using Chat.Infrastructure.Abstractions.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services;

/// <inheritdoc/>
public class AccountService :
    IAccountService
{
    private readonly IRepository<User> _userRepository;
    private readonly IAuthService _authService;
    private readonly IPasswordManager _passwordManager;

    public AccountService(IRepository<User> userRepository, IAuthService authService, IPasswordManager passwordManager)
    {
        _userRepository = userRepository;
        _authService = authService;
        _passwordManager = passwordManager;
    }

    /// <inheritdoc/>
    public async Task<UserResponse> GetUserAsync(long id, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw NotFoundException.UserNotFound();

        return user.Map();
    }

    /// <inheritdoc/>
    public async Task<UserResponse> GetCurrentUserAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetUserIdFromHttpRequest(request);
        var user = await GetUserAsync(userId, cancellationToken);

        return user;
    }

    /// <inheritdoc/>
    public async Task<UserResponse> RegisterAsync(RegisterRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository
            .Where(u => u.Nickname == request.Nickname)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
            throw ConflictException.UserAlreadyExists();

        var hash = _passwordManager.HashPassword(request.Password);
        var newUser = new User
        {
            Nickname = request.Nickname,
            PasswordHash = hash
        };

        var savedUser = await _userRepository.AddAsync(newUser, cancellationToken);
        await _authService.GenerateAndSetTokensAsync(savedUser, response, cancellationToken);

        return savedUser.Map();
    }

    /// <inheritdoc/>
    public async Task LoginAsync(LoginRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .AsQuery()
            .SingleOrDefaultAsync(u => u.Nickname == request.Nickname, cancellationToken)
            ?? throw UnauthorizedException.WrongNickname();

        if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash))
            throw UnauthorizedException.WrongPassword();

        await _authService.GenerateAndSetTokensAsync(user, response, cancellationToken);
    }

    /// <inheritdoc/>
    public Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
        => _authService.ClearTokensAsync(request, response, cancellationToken);

    /// <inheritdoc/>
    public async Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
        => await _authService.RefreshAccessTokenAsync(request, response, cancellationToken);
}
