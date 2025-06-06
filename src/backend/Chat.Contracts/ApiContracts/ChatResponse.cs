namespace Chat.Contracts.ApiContracts
{
    public record ChatRoomResponse
    {
        public long Id { get; init; }
        public required string Title { get; init; }
        public required UserResponse Owner { get; init; }
    }
}
