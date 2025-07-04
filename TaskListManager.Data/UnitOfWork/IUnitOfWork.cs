using TaskListManager.Data.Repository;

namespace TaskListManager.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task<bool> Complete(CancellationToken cancellationToken = default(CancellationToken));
    }
}
