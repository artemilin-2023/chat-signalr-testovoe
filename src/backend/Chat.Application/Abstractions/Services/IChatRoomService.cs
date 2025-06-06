using Chat.Application.Models.Pagination;
using Chat.Contracts.ApiContracts;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Abstractions.Services
{
    /// <summary>
    /// Интерфейс сервиса для управления чатами
    /// </summary>
    public interface IChatRoomService
    {
        /// <summary>
        /// Получает список чатов с пагинацией
        /// </summary>
        /// <param name="pagination">Параметры пагинации</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Постраничный список чатов</returns>
        public Task<Paginated<ChatRoomResponse>> GetChatsAsync(PaginationParams pagination, CancellationToken cancellationToken);

        /// <summary>
        /// Получает список чатов с пагинацией для текущего пользователя
        /// </summary>
        /// <param name="httpRequest">HTTP-запрос для получения информации о текущем пользоавтеле</param>
        /// <param name="pagination">Параметры пагинации</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Постраничный список чатов пользователя</returns>
        public Task<Paginated<ChatRoomResponse>> GetChatsForUser(HttpRequest httpRequest, PaginationParams pagination, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получает информацию о конкретной чатовпо идентификатору
        /// </summary>
        /// <param name="id">Идентификатор чата</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Данные о запрошенном чате</returns>
        public Task<ChatRoomResponse> GetChatAsync(long id, CancellationToken cancellationToken);
        
        /// <summary>
        /// Создает новый чат
        /// </summary>
        /// <param name="request">Данные для создания чата</param>
        /// <param name="httpRequest">HTTP-запрос для получения информации о создателе</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Данные о созданном чате</returns>
        public Task<ChatRoomResponse> CreateChatAsync(CreateChatRequest request, HttpRequest httpRequest, CancellationToken cancellationToken);
    }
}