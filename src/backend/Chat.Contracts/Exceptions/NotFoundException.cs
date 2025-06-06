using System.Net;

namespace Chat.Contracts.Exceptions
{
    public class NotFoundException(string message) : HttpExceptionBase (message, (int)HttpStatusCode.NotFound)
    {
        public static NotFoundException UserNotFound(string message = ErrorMessages.NotFound.User)
            => new(message);

        public static NotFoundException ChatNotFound(string message = ErrorMessages.NotFound.Chat)
            => new(message);
    }
}
