using Chat.API.Hubs.Abstractions;
using Chat.Application.Abstractions.Services;
using Chat.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs
{
    [Authorize, HubRoute("/chat")]
    public class ChatHub(IAuthService authService, IMessageService messageService, ILogger<ChatHub> logger) :
        Hub<IChatClient>, IChatServer
    {
        private readonly IAuthService _authService = authService;
        private readonly IMessageService _messageService = messageService;
        private readonly ILogger<ChatHub> _logger = logger;

        /// <inheritdoc/>
        public async Task JoinChat(long chatId)
        {
            var userId = GetCurrentUser();
            _logger.LogInformation("Adding new user with id '{userId}' to chat with id '{chatId}'",
                userId, chatId);

            await Groups.AddToGroupAsync(userId.ToString(), chatId.ToString());
        }

        private long GetCurrentUser()
        {
            var httpContext = Context.GetHttpContext()
                ?? throw new InvalidOperationException("Failed to get current HttpContext");
            var userId = _authService.GetUserIdFromHttpRequest(httpContext.Request);
            
            return userId;
        }

        /// <inheritdoc/>
        public async Task LeftChat(long chatId)
        {
            var userId = GetCurrentUser();
            _logger.LogInformation("Removing user with id '{userId}' from chat with id '{chatId}'",
                userId, chatId);

            await Groups.RemoveFromGroupAsync(userId.ToString(), chatId.ToString());
        }

        /// <inheritdoc/>
        public async Task SendMessage(long chatId, string message)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(CommonConstants.WaitBeforeCancel);

            var userId = GetCurrentUser();
            _logger.LogDebug("User {user} send message '{msg}' to chat '{chat}'",
                userId, message, chatId);

            var messageEntity = await _messageService.SaveMessageAsync(chatId, userId, message, tokenSource.Token);

            await Clients.Group(chatId.ToString())
                .ReceiveMessage(messageEntity);
        }
    }
}