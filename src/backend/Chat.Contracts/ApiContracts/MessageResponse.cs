namespace Chat.Contracts.ApiContracts
{
    public record MessageResponse
    {
        public long Id { get; init; }
        public required string Text { get; init; }
        public DateTime SentAt { get; init; }
        public required UserResponse Sender { get; init; }
    }
}
