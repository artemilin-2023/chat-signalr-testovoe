namespace Chat.API.Hubs.Abstractions
{
    /// <summary>
    /// Интерфейс сервера чата для использования в SignalR хабах
    /// </summary>
    /// <remarks>
    /// Определяет методы, которые могут быть вызваны на сервере клиентами
    /// </remarks>
    public interface IChatServer
    {
        /// <summary>
        /// Отправляет сообщение в указанную чат-комнату
        /// </summary>
        /// <param name="chatId">Идентификатор чат-комнаты</param>
        /// <param name="message">Текст сообщения для отправки</param>
        /// <returns>Task, представляющий асинхронную операцию</returns>
        Task SendMessage(long chatId, string message);

        /// <summary>
        /// Присоединяет пользователя к указанной чат-комнате
        /// </summary>
        /// <param name="chatId">Идентификатор чат-комнаты для присоединения</param>
        /// <returns>Task, представляющий асинхронную операцию</returns>
        Task JoinChat(long chatId);

        /// <summary>
        /// Отсоединяет пользователя от указанной чат-комнаты
        /// </summary>
        /// <param name="chatId">Идентификатор чат-комнаты для отсоединения</param>
        /// <returns>Task, представляющий асинхронную операцию</returns>
        Task LeftChat(long chatId);
    }
}
