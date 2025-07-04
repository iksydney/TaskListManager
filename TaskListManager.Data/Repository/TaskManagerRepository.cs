using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskListManager.Data.DataAccess;

namespace TaskListManager.Data.Repository
{
    internal class TaskManagerRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        internal DbSet<TEntity> dbSet;
        private bool _disposed;
        public TaskManagerRepository(AppDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> Get(int id)
        {

            return await context.Set<TEntity>().FindAsync(id);
        }


        public List<TEntity> GetAllNoTracking(Expression<Func<TEntity, bool>> keySelector)
        {
            return context.Set<TEntity>().AsNoTracking().Where(keySelector).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsNoTracking()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public int AddAndReturnId(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            var entityId = (int)typeof(TEntity).GetProperty("Id").GetValue(entity);

            return entityId;
        }
        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().UpdateRange(entities);

            foreach (var entity in entities)
            {
                context.Entry(entity).State = EntityState.Modified;
            }
        }
        public async Task SaveChanges(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            foreach (var prop in properties)
            {
                context.Entry(entity).Property(prop).IsModified = true;
            }
            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;
        }
        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = dbSet.Where(predicate);
            return entities.Any();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().CountAsync(filter);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }

        public virtual IEnumerable<T> SqlQuery<T>(String sql, params object[] parameters) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
        }
        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return GetAll(pageIndex, pageSize, keySelector, null, orderBy);
        }

        public async Task<IQueryable<TEntity>> AsyncGetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return await AsyncGet(pageIndex, pageSize, keySelector, null, orderBy);
        }

        public IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties)
        {
            var entities = FilterQueryString(keySelector, predicate, orderBy, includeProperties);
            return entities.Paginate(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            return entities.Paginate(pageIndex, pageSize);
        }

        public async Task<IQueryable<TEntity>> AsyncGet(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = await AsyncFilterQuery(keySelector, predicate, orderBy, includeProperties);
            return entities.Paginate(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return dbSet.ToListAsync();
        }
        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> keySelector)
        {
            return await dbSet.FirstOrDefaultAsync(keySelector);
        }

        public async Task<TEntity> GetByIdIncludingAsync(Expression<Func<TEntity, bool>> keySelector, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return await entities.FirstOrDefaultAsync(keySelector);
        }

        public IQueryable<TEntity> FindIncludingAsync(Expression<Func<TEntity, bool>> keySelector, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.Where(keySelector);
        }

        public IEnumerable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public async Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> GetAllAsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return await GetAllAsync(pageIndex, pageSize, keySelector, null, orderBy);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector,
            Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            return entities.Skip(pageIndex).Take(pageSize);
        }


        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.ToListAsync();
        }

        private IQueryable<TEntity> FilterQueryString(Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
         string[] includeProperties)
        {
            var entities = IncludeStringProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }

        private IQueryable<TEntity> FilterQuery(Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
         Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }

        private async Task<IQueryable<TEntity>> AsyncFilterQuery(Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
         Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return await Task.FromResult(entities);
        }

        private IQueryable<TEntity> IncludeStringProperties(string[] includeProperties)
        {
            IQueryable<TEntity> entities = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                entities = dbSet.Include(includeProperty).Include(includeProperty);
            }
            return entities;
        }


        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = dbSet;
            //Todo: a better way to write this
            //http://appetere.com/post/Passing-Include-statements-into-a-Repository
            //not tested before will do that later looks shorter
            return includeProperties.Aggregate(entities, (current, include) => current.Include(include));
            //will make this code snippet obsolete later
            //foreach (var includeProperty in includeProperties)
            //{
            //    entities = entities.Include(includeProperty);
            //}
            //return entities;
        }

        public virtual IQueryable<TEntity> Table()
        {
            IQueryable<TEntity> query = dbSet;
            return query;
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = dbSet;
            return query.Any(filter);
        }
        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return dbSet.Where(where).ToList();
        }
        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return dbSet.Where(where).AsQueryable();
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return filter == null ? await query.AnyAsync() : await query.AnyAsync(filter);
        }
        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }
        public TEntity GetLastOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null) query = query.Where(filter);

            return query.ToList().LastOrDefault();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                context.Dispose();
            }

            _disposed = true;
        }
        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            return await context.Set<TEntity>().Where(match).ToListAsync();
        }

        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim()))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public IEnumerable<TEntity> ExecuteSqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return context.Set<TEntity>().FromSqlRaw(sql, parameters).ToList();
        }

        public async Task<IEnumerable<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return await context.Set<TEntity>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public TEntity ExecuteSqlQuerySingle<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return context.Set<TEntity>().FromSqlRaw(sql, parameters).FirstOrDefault();
        }

        public async Task<TEntity> ExecuteSqlQuerySingleAsync<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return await context.Set<TEntity>().FromSqlRaw(sql, parameters).FirstOrDefaultAsync();
        }

        public IEnumerable<TEntity> GetWithInclude(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, IEnumerable<TEntity>>> includeOther = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (includeOther != null)
                query = query.Include(includeOther);

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public void Detach(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Detached;
        }
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
    }
}
