using System.Threading;
using System.Threading.Tasks;

namespace Msi.AspNetCore.UnitOfWork
{
    public interface IUnitOfWork<TDataContext> where TDataContext : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
