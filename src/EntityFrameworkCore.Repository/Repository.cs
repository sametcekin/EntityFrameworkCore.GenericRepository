using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EFCoreGenericRepository
{
    public class Repository<TContext, TEntity> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class, new()
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public async Task AddAsync(TEntity entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task<TEntity> AddAndReturnEntityAsync(TEntity entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            return entity;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            _context.SaveChanges();
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            if (properties.Any(x => x.Name.Contains("IsDeleted")))
            {
                var propertyInfo = properties.Where(x => x.Name == "IsDeleted").FirstOrDefault();
                propertyInfo.SetValue(entity, Convert.ChangeType(true, propertyInfo.PropertyType), null);
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            _context.SaveChanges();
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken token = default)
        {
            var properties = entity.GetType().GetProperties();
            if (properties.Any(x => x.Name.Contains("IsDeleted")))
            {
                var propertyInfo = properties.Where(x => x.Name == "IsDeleted").FirstOrDefault();
                propertyInfo.SetValue(entity, Convert.ChangeType(true, propertyInfo.PropertyType), null);
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            var properties = entities?.FirstOrDefault()?.GetType().GetProperties();
            if (properties.Any(x => x.Name.Contains("IsDeleted")))
            {
                var propertyInfo = properties.Where(x => x.Name == "IsDeleted").FirstOrDefault();
                foreach (var entity in entities)
                {
                    propertyInfo.SetValue(entity, Convert.ChangeType(true, propertyInfo.PropertyType), null);
                    _dbSet.Update(entity);
                }
            }
            else
            {
                _dbSet.RemoveRange(entities);
            }

            _context.SaveChanges();
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            var properties = entities.FirstOrDefault()?.GetType().GetProperties();
            if (properties.Any(x => x.Name.Contains("IsDeleted")))
            {
                var propertyInfo = properties.Where(x => x.Name == "IsDeleted").FirstOrDefault();
                foreach (var entity in entities)
                {
                    propertyInfo.SetValue(entity, Convert.ChangeType(true, propertyInfo.PropertyType), null);
                    _dbSet.Update(entity);
                }
            }
            else
            {
                _dbSet.RemoveRange(entities);
            }

            await _context.SaveChangesAsync();
        }

        public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (include != null) query = include(query);

            return disableTracking ? query.AsNoTracking().FirstOrDefault() : query.FirstOrDefault();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (include != null) query = include(query);

            return disableTracking ? await query.AsNoTracking().FirstOrDefaultAsync() : await query.FirstOrDefaultAsync();
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (include != null) query = include(query);

            return disableTracking ? query.AsNoTracking().ToList() : query.ToList();
        }

        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (include != null) query = include(query);

            return disableTracking ? query.AsNoTracking().ToListAsync() : query.ToListAsync();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (include != null) query = include(query);

            return disableTracking ? query.AsNoTracking() : query;
        }
    }
}
