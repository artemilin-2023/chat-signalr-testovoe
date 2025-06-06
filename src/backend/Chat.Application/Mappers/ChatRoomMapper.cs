using Chat.Contracts.ApiContracts;
using Chat.Domain;

namespace Chat.Application.Mappers
{
    internal static class ChatRoomMapper
    {
        internal static ChatRoomResponse Map(this ChatRoom chat)
            => new() 
            { 
                Id = chat.Id, 
                Title = chat.Title,
                Owner = chat.Owner.Map()
            };
    }
}
