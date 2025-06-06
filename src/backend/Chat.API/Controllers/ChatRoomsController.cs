using Chat.Application.Abstractions.Services;
using Chat.Application.Models.Pagination;
using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Chat.API.Controllers;

/// <summary>
/// Контроллер для управления чатами
/// </summary>
/// <remarks>
/// Позволяет создавать новые чаты, получать данные о существующих и просматривать список с пагинацией
/// </remarks>
[ApiController]
[Route("chatrooms"), Authorize]
public class ChatRoomsController(IChatRoomService service) : ControllerBase
{
    private readonly IChatRoomService _service = service;

    /// <summary>
    /// Получает информацию о чат-комнате по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор чат-комнаты</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Детальная информация о чат</returns>
    /// <response code="200">Возвращает данные о запрошенном чате</response>
    /// <response code="404">Если чат не найден</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ChatRoomResponse))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorMessage))]
    public async Task<IActionResult> GetById([FromRoute, BindRequired] long id, CancellationToken cancellationToken)
    {
        var chat = await _service.GetChatAsync(id, cancellationToken);
        return Ok(chat);
    }

    /// <summary>
    /// Создает новую чат-комнату
    /// </summary>
    /// <param name="request">Данные для создания чат-комнаты</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о созданном чате</returns>
    /// <response code="201">Возвращает данные о созданном чате</response>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(ChatRoomResponse))]
    public async Task<IActionResult> AddChatRoom([FromBody, BindRequired] CreateChatRequest request, CancellationToken cancellationToken)
    {
        var chat = await _service.CreateChatAsync(request, Request, cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { id = chat.Id }, chat);
    }

    /// <summary>
    /// Получает список чатов с пагинацией
    /// </summary>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Постраничный список чатов</returns>
    /// <response code="200">Возвращает список чатов с информацией о пагинации</response>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Paginated<ChatRoomResponse>))]
    public async Task<IActionResult> GetChatRoomsWithPagination([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
    {
        var chats = await _service.GetChatsAsync(pagination, cancellationToken);
        
        return Ok(chats);
    }

    /// <summary>
    /// Получает список чатов с пагинацией для текущего пользователя
    /// </summary>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Постраничный список чатов</returns>
    /// <response code="200">Возвращает список чатов с информацией о пагинации</response>
    [HttpGet("my")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Paginated<ChatRoomResponse>))]
    public async Task<IActionResult> GetChatRoomsForCurrentUser([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
    {
        var chats = await _service.GetChatsForUser(Request, pagination, cancellationToken);
        
        return Ok(chats);
    }
}
