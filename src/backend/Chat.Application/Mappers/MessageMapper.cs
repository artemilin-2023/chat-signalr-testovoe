using Chat.Contracts.ApiContracts;
using Chat.Domain;

namespace Chat.Application.Mappers
{
    internal static class MessageMapper
    {
        internal static MessageResponse Map(this Message message)
            => new()
            {
                Id = message.Id,
                Text = message.Text,
                Sender = message.Sender.Map(),
                SentAt = message.SentAt
            };
    }
}
