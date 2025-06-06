using Chat.Contracts.ApiContracts;
using Chat.Domain;

namespace Chat.Application.Mappers
{
    internal static class UserMapper
    {
        internal static UserResponse Map(this User user)
            => new(user.Id, user.Nickname);
    }
}
