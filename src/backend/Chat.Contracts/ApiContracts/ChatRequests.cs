namespace Chat.Contracts.ApiContracts
{
    public record CreateChatRequest
    {
        public required string Title { get; init; }
    }
}
