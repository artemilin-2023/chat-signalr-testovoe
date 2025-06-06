using Chat.Application.Abstractions.Services;
using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Chat.API.Controllers
{
    /// <summary>
    /// Контроллер для управления сообщениями в чатах
    /// </summary>
    /// <remarks>
    /// Предоставляет API для получения сообщений из чатов с обратной пагинацией
    /// </remarks>
    [ApiController]
    [Route("chatrooms/{chatId:long}/messages"), Authorize]
    public class MessageController(IMessageService messageService) : ControllerBase
    {
        private readonly IMessageService _messageService = messageService;

        /// <summary>
        /// Получает сообщения из чата с использованием обратной пагинации
        /// </summary>
        /// <param name="chatId">Идентификатор чат-комнаты</param>
        /// <param name="pagination">Параметры обратной пагинации (от какого сообщения и лимит)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список сообщений с информацией о пагинации</returns>
        /// <response code="200">Возвращает сообщения из указанной чат-комнаты</response>
        /// <response code="401">Если пользователь не авторизован</response>
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(BackwardPaginated<MessageResponse>))]
        public async Task<IActionResult> GetMessagesWithPagination([FromRoute, BindRequired] long chatId, [FromQuery] BackwardPaginationParams pagination, CancellationToken cancellationToken)
        {
            var messages = await _messageService.GetMessagesAsync(chatId, pagination, cancellationToken);
            return Ok(messages);
        }
    }
}
