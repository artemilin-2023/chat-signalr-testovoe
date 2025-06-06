namespace Chat.Contracts.ApiContracts
{
    public record BackwardPaginationParams
    {
        public long? Before { get; init; }
        public int Limit { get; init; } = 50;
    }
}
