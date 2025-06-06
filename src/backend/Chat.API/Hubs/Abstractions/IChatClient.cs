using Chat.Contracts.ApiContracts;

namespace Chat.API.Hubs.Abstractions
{
    /// <summary>
    /// Интерфейс клиента чата для использования в SignalR хабах
    /// </summary>
    /// <remarks>
    /// Определяет методы, которые могут быть вызваны на клиенте сервером
    /// </remarks>
    public interface IChatClient
    {
        /// <summary>
        /// Метод для получения нового сообщения клиентом
        /// </summary>
        /// <param name="message">Информация о полученном сообщении</param>
        /// <returns>Task, представляющий асинхронную операцию</returns>
        Task ReceiveMessage(MessageResponse message);
    }
}
