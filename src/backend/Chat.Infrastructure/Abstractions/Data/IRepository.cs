using System.Linq.Expressions;

namespace Chat.Infrastructure.Abstractions.Data;

/// <summary>
/// Обобщенный интерфейс репозитория для работы с сущностями
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IRepository<TEntity>
{
    /// <summary>
    /// Добавляет сущность в репозиторий
    /// </summary>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Добавленная сущность</returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    /// <summary>
    /// Добавляет коллекцию сущностей в репозиторий
    /// </summary>
    /// <param name="entities">Коллекция сущностей для добавления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Коллекция добавленных сущностей</returns>
    Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет сущность в репозитории
    /// </summary>
    /// <param name="entity">Сущность с обновленными данными</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленная сущность</returns>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет сущность из репозитория
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает запрос для получения коллекции сущностей
    /// </summary>
    /// <param name="tracking">Флаг отслеживания изменений сущностей</param>
    /// <returns>Запрос IQueryable для дальнейшей фильтрации</returns>
    IQueryable<TEntity> AsQuery(bool tracking = false);
    
    /// <summary>
    /// Фильтрует сущности по указанному предикату
    /// </summary>
    /// <param name="expression">Выражение для фильтрации сущностей</param>
    /// <returns>Отфильтрованный запрос IQueryable</returns>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
}
