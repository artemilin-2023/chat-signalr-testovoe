using System.Net;

namespace Chat.Contracts.Exceptions
{
    public class UnauthorizedException(string message) : HttpExceptionBase(message, (int)HttpStatusCode.Unauthorized)
    {
        public static UnauthorizedException WrongPassword(string message = ErrorMessages.Unauthorized.WrongPassword)
            => new(message);

        public static UnauthorizedException WrongNickname(string message = ErrorMessages.Unauthorized.WrongNickname)
            => new(message);

        public static UnauthorizedException InvalidToken(string message = ErrorMessages.Unauthorized.InvalidToken)
            => new(message);

        public static UnauthorizedException InvalidHeader(string message = ErrorMessages.Unauthorized.InvalidAuthorizationHeader)
            => new(message);

        public static UnauthorizedException TokenExpaired(string message = ErrorMessages.Unauthorized.TokenExpaired)
            => new(message);
    }
}
