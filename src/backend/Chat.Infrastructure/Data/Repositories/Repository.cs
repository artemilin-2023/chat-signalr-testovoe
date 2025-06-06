using Chat.Infrastructure.Abstractions.Data;
using Chat.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Data.Repositories;

public class Repository<TEntity>(DataContext context, ILogger<Repository<TEntity>> logger) : 
    IRepository<TEntity>
    where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    protected readonly ILogger<Repository<TEntity>> _logger = logger;

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Entity added: {Entity}", entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("{Count} entities added", entities.Count());
        return entities;
    }

    public IQueryable<TEntity> AsQuery(bool tracking = false)
    {
        if (tracking)
            return _dbSet.AsQueryable();
        return _dbSet.AsNoTracking().AsQueryable();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Entity updated: {Entity}", entity);
        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Entity deleted: {entity}", typeof(TEntity).Name);
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _dbSet.Where(expression);
    }
}