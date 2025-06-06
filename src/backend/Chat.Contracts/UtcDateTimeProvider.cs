using Chat.Contracts.Abstractions;

namespace Chat.Contracts
{
    /// <inheritdoc/>
    internal class UtcDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc/>
        public DateTime Now => DateTime.UtcNow;
    }
}
