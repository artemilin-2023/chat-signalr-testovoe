using Chat.Contracts.ApiContracts;

namespace Chat.Application.Abstractions.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с сообщениями
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Сохраняет новое сообщение
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="sendFromId">Идентификатор отправителя сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Информация о сохраненном сообщении</returns>
        Task<MessageResponse> SaveMessageAsync(long chatId, long sendFromId, string message, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получает список сообщений из чата с обратной пагинацией
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="pagination">Параметры обратной пагинации</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список сообщений с информацией о пагинации</returns>
        Task<BackwardPaginated<MessageResponse>> GetMessagesAsync(long chatId, BackwardPaginationParams pagination, CancellationToken cancellationToken);
    }
}
