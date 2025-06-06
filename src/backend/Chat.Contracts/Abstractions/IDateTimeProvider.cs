namespace Chat.Contracts.Abstractions
{
    /// <summary>
    /// Интерфейс для предоставления текущего времени и даты
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Получает текущее время и дату
        /// </summary>
        /// <value>Экземпляр DateTime, представляющий текущее время</value>
        DateTime Now { get; }
    }
}
