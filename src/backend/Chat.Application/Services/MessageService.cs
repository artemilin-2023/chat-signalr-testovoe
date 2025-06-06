using Chat.Application.Abstractions.Services;
using Chat.Application.Mappers;
using Chat.Contracts.Abstractions;
using Chat.Contracts.ApiContracts;
using Chat.Contracts.Exceptions;
using Chat.Domain;
using Chat.Infrastructure.Abstractions.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services
{
    /// <inheritdoc/>
    internal class MessageService :
        IMessageService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ChatRoom> _chatRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IDateTimeProvider _dateTime;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IRepository<User> userRepository, IRepository<ChatRoom> chatRepository, IRepository<Message> messageRepository, IDateTimeProvider dateTime, ILogger<MessageService> logger)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _dateTime = dateTime;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BackwardPaginated<MessageResponse>> GetMessagesAsync(long chatId, BackwardPaginationParams pagination, CancellationToken cancellationToken)
        {
            var query = _messageRepository
                .AsQuery()
                .Include(m => m.Sender)
                .Where(m => m.Chat.Id == chatId);

            if (pagination.Before.HasValue)
            {
                var beforeMessage = await _messageRepository
                    .Where(m => m.Id == pagination.Before)
                    .FirstAsync(cancellationToken);
                
                if (beforeMessage is not null)
                {
                    query = query.Where(m => m.SentAt < beforeMessage.SentAt);
                }
            }

            var messages = await query
                .OrderByDescending(m => m.SentAt)
                .Take(pagination.Limit + 1)
                .Select(m => m.Map())
                .ToListAsync(cancellationToken);

            var hasMore = messages.Count > pagination.Limit;

            if (hasMore)
                messages.RemoveAt(messages.Count - 1);

            var paginationResult = new BackwardPaginated<MessageResponse>()
            {
                HasMore = hasMore,
                Items = messages
            };

            return paginationResult;
        }

        /// <inheritdoc/>
        public async Task<MessageResponse> SaveMessageAsync(long chatId, long sendFromId, string message, CancellationToken cancellationToken)
        {
            var sender = await _userRepository
                .Where(u => u.Id == sendFromId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new InvalidOperationException($"Failed to save message: user with id '{sendFromId}' not found.");

            var chat = await _chatRepository
                .Where(u => u.Id == chatId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw NotFoundException.ChatNotFound($"Failed to send message: chat with id '{chatId}' not found.");

            var messageEntity = new Message()
            {
                Chat = chat,
                Sender = sender,
                Text = message,
                SentAt = _dateTime.Now
            };

            await _messageRepository.AddAsync(messageEntity, cancellationToken);
            return messageEntity.Map();
        }
    }
}
