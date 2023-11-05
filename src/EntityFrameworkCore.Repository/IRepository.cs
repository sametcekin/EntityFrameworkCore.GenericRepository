using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EFCoreGenericRepository
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// class : reference type
    public interface IRepository<T> where T : class, new()
    {
        #region add

        void Add(T entity);
        void Add(IEnumerable<T> entities);
        Task AddAsync(T entity, CancellationToken token = default);
        Task AddAsync(IEnumerable<T> entities, CancellationToken token = default);

        #endregion

        #region update

        void Update(T entity);
        void Update(IEnumerable<T> entities);
        Task UpdateAsync(T entity, CancellationToken token = default);
        Task UpdateAsync(IEnumerable<T> entities, CancellationToken token = default);

        #endregion

        #region delete

        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        Task DeleteAsync(T entity, CancellationToken token = default);
        Task DeleteAsync(IEnumerable<T> entities, CancellationToken token = default);

        #endregion

        T GetFirstOrDefault(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = true);
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true);
        List<T> GetList(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true);
        Task<List<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default);
        IQueryable<T> GetQueryable(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true);
    }
}
