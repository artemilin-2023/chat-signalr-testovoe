namespace Chat.Contracts.Exceptions
{
    internal static class ErrorMessages
    {
        internal static class NotFound
        {
            internal const string User = "User not found. Please check the nickname and try again.";
            internal const string Chat = "Chat not found.";
        }

        internal static class Conflict
        {
            internal const string User = "User with this nickname already exists.";
        }

        internal static class Unauthorized
        {
            internal const string WrongNickname = "Nickname not found. Please check and try again.";
            internal const string WrongPassword = "Incorrect password. Please try again.";
            internal const string InvalidAuthorizationHeader = "These auth headers are not allowed.";
            internal const string InvalidToken = "Invalid authorization token. Please authenticate again.";
            internal const string TokenExpaired = "Authorization token has expired. Please log in again.";
        }
    }
}
