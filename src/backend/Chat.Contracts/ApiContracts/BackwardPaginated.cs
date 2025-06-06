namespace Chat.Contracts.ApiContracts
{
    public record BackwardPaginated<TData>
    {
        public bool HasMore { get; init; }
        public required IReadOnlyList<TData> Items { get; init; }
    }
}
