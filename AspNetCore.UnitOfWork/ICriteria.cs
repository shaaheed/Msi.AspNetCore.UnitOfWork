using System.Linq;

namespace AspNetCore.UnitOfWork
{
    public interface ICriteria<TEntity> where TEntity : IEntity
    {

        IQueryable<TEntity> Execute(IQueryable<TEntity> entities);

    }
}
