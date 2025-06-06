namespace Chat.Contracts.Exceptions
{
    public abstract class HttpExceptionBase(string message, int statusCode) : Exception(message)
    {
        public int StatusCode { get; private set; } = statusCode;
    }
}
