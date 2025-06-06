namespace Chat.Contracts.ApiContracts
{
    public record ErrorMessage
    {
        public required string Message { get; init; }
    }

    public record SignalrErrorMessage
    {
        public required string Message { get; init; }
        public required int StatusCode { get; init; }
    }
}
