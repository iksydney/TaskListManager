using Microsoft.EntityFrameworkCore;
using TaskListManager.Data.DataAccess;
using TaskListManager.Data.Repository;

namespace TaskListManager.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public Dictionary<Type, object> Repositories
        {
            get => _repositories;
            set => Repositories = value;
        }
        public UnitOfWork(AppDbContext context)
        {

            _context = context;
        }

        public async Task<bool> Complete(CancellationToken cancellationToken = default(CancellationToken))
        {

            return await _context.SaveChangesAsync() > 0;
        }
        public IRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new TaskManagerRepository<T>(_context);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
