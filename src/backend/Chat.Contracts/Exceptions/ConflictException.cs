using System.Net;

namespace Chat.Contracts.Exceptions
{
    public class ConflictException(string message) : HttpExceptionBase(message, (int)HttpStatusCode.Conflict)
    {
        public static ConflictException UserAlreadyExists(string message = ErrorMessages.Conflict.User)
            => new(message);
    }
}
