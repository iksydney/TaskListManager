using System.Linq.Expressions;

namespace TaskListManager.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll();
        void RemoveRange(IEnumerable<TEntity> entities);
        bool Exist(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);

        IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> GetAllAsNoTracking();
        IEnumerable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<IQueryable<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Int32>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> keySelector);
        Task<TEntity> GetByIdIncludingAsync(Expression<Func<TEntity, bool>> keySelector, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindIncludingAsync(Expression<Func<TEntity, bool>> keySelector, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllAsQueryable(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAllNoTracking(Expression<Func<TEntity, bool>> keySelector);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<T> SqlQuery<T>(String sql, params object[] parameters) where T : new();
        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>

        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity GetLastOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        void Update(TEntity entity);
        Task SaveChanges(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? page = null, int? pageSize = null);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match);
        IEnumerable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, IEnumerable<TEntity>>> includeOther = null, string includeProperties = "");
        Task<IQueryable<TEntity>> AsyncGetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        void Detach(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
        void UpdateRange(IEnumerable<TEntity> entities);
        int AddAndReturnId(TEntity entity);
        Task<TEntity> ExecuteSqlQuerySingleAsync<TEntity>(string sql, params object[] parameters) where TEntity : class;
        TEntity ExecuteSqlQuerySingle<TEntity>(string sql, params object[] parameters) where TEntity : class;
        Task<IEnumerable<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql, params object[] parameters) where TEntity : class;
        IEnumerable<TEntity> ExecuteSqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class;
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        //IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
    }
}
