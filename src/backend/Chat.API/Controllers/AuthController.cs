using Chat.Application.Abstractions.Services;
using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Chat.API.Controllers;

/// <summary>
/// Контроллер для управления аутентификацией и авторизацией пользователей
/// </summary>
/// <remarks>
/// Предоставляет API для регистрации, входа, выхода пользователей и управления токенами авторизации
/// </remarks>
[ApiController]
[Route("account")]
public class AuthController(IAccountService studentService) : ControllerBase
{
    private readonly IAccountService accountService = studentService;

    /// <summary>
    /// Получает информацию о текущем авторизованном пользователе
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о пользователе</returns>
    /// <response code="200">Возвращает данные об авторизованном пользователе</response>
    /// <response code="401">Если пользователь не авторизован</response>
    /// <response code="404">Если пользователь не найден</response>
    [HttpGet("me"), Authorize]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserResponse))]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ErrorMessage))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorMessage))]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var user = await accountService.GetCurrentUserAsync(Request, cancellationToken);

        return Ok(user);
    }

    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <param name="request">Данные для регистрации пользователя</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о зарегистрированном пользователе</returns>
    /// <response code="201">Возвращает данные о созданном пользователе</response>
    /// <response code="409">Если пользователь с таким именем существует</response>
    [HttpPost("register")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(UserResponse))]
    [ProducesResponseType(statusCode: StatusCodes.Status409Conflict, type: typeof(ErrorMessage))]
    public async Task<IActionResult> Register([FromBody, BindRequired] RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = await accountService.RegisterAsync(request, Response, cancellationToken);
        
        return CreatedAtAction(nameof(GetCurrentUser), user);
    }

    /// <summary>
    /// Выполняет вход пользователя в систему
    /// </summary>
    /// <param name="request">Данные для входа (никнейм и пароль)</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат операции входа</returns>
    /// <response code="200">Успешный вход в систему</response>
    /// <response code="401">Если предоставлены неверные учетные данные</response>
    [HttpPost("login")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ErrorMessage))]
    public async Task<IActionResult> Login([FromBody, BindRequired] LoginRequest request, CancellationToken cancellationToken)
    {
        await accountService.LoginAsync(request, Response, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Выполняет выход пользователя из системы
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат операции выхода</returns>
    /// <response code="200">Успешный выход из системы</response>
    [HttpGet("logout")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await accountService.LogoutAsync(Request, Response, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновляет токен доступа пользователя
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат обновления токена</returns>
    /// <response code="200">Успешное обновление токена доступа</response>
    /// <response code="401">Если refresh token недействителен или отсутствует</response>
    [HttpGet("token/refresh")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ErrorMessage))]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        await accountService.RefreshToken(Request, Response, cancellationToken);

        return Ok();
    }
}
