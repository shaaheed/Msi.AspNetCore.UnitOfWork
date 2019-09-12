using System.Linq;

namespace Msi.AspNetCore.UnitOfWork
{
    public interface ICriteria<TEntity> where TEntity : class
    {

        IQueryable<TEntity> Execute(IQueryable<TEntity> entities);

    }
}
