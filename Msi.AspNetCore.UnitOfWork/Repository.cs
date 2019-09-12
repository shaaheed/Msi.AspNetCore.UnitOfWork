using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Msi.AspNetCore.UnitOfWork
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly IDataContext _dataContext;

        public Repository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dataContext.Set<TEntity>().AsQueryable();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().CountAsync(predicate, cancellationToken);
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().LongCountAsync(predicate, cancellationToken);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public TEntity Remove(TEntity entity)
        {
            return _dataContext.Set<TEntity>().Remove(entity).Entity;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dataContext.Set<TEntity>().RemoveRange(entities);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<TEntity>().SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public TEntity Update(TEntity entity)
        {
            return _dataContext.Set<TEntity>().Update(entity).Entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dataContext.Set<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> Match(ICriteria<TEntity> criteria, bool readOnly = true)
        {
            return criteria.Execute(_dataContext.Set<TEntity>());
        }
    }
}
