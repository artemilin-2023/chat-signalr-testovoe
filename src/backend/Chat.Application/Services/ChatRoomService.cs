using Chat.Application.Abstractions.Services;
using Chat.Application.Extensions;
using Chat.Application.Mappers;
using Chat.Application.Models.Pagination;
using Chat.Contracts.ApiContracts;
using Chat.Contracts.Exceptions;
using Chat.Domain;
using Chat.Infrastructure.Abstractions.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services
{
    /// <inheritdoc/>
    internal class ChatRoomService :
        IChatRoomService
    {
        private readonly IAuthService _authService;
        private readonly IRepository<ChatRoom> _chatRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<ChatRoomService> _logger;

        public ChatRoomService(IAuthService authService, IRepository<User> userRepository, IRepository<ChatRoom> chatRepository, ILogger<ChatRoomService> logger)
        {
            _authService = authService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ChatRoomResponse> CreateChatAsync(CreateChatRequest request, HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            var userId = _authService.GetUserIdFromHttpRequest(httpRequest);
            var user = await _userRepository
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new InvalidOperationException($"Failed to create new chat: cannot find user with id '{userId}'");

            var chat = new ChatRoom()
            {
                Title = request.Title,
                Members = [user],
                Owner = user
            };

            await _chatRepository.AddAsync(chat, cancellationToken);
            return chat.Map();
        }

        /// <inheritdoc/>
        public async Task<ChatRoomResponse> GetChatAsync(long id, CancellationToken cancellationToken)
        {
            var chat = await _chatRepository
                .AsQuery()
                .Include(c => c.Owner)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw NotFoundException.ChatNotFound();

            return chat.Map();
        }

        /// <inheritdoc/>
        public async Task<Paginated<ChatRoomResponse>> GetChatsAsync(PaginationParams pagination, CancellationToken cancellationToken)
        {
            var chats = _chatRepository
                .AsQuery()
                .Include(c => c.Owner);

            return await chats.AsPaginatedAsync(pagination, map: c => c.Map(), cancellationToken);
        }

        public async Task<Paginated<ChatRoomResponse>> GetChatsForUser(HttpRequest httpRequest, PaginationParams pagination, CancellationToken cancellationToken)
        {
            var userId = _authService.GetUserIdFromHttpRequest(httpRequest);

            var chats = _chatRepository
                .AsQuery()
                .Include(c => c.Owner)
                .Where(c => c.Owner.Id == userId);

            return await chats.AsPaginatedAsync(pagination, map: c => c.Map(), cancellationToken);
        }
    }
}
