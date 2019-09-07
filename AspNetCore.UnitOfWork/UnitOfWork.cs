using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.UnitOfWork
{
    public class UnitOfWork<TDataContext> : IUnitOfWork<TDataContext> where TDataContext : IDataContext
    {

        private readonly IDataContext _dataContext;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _repositories = new Dictionary<Type, object>();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TSet> GetRepository<TSet>() where TSet : class, IEntity
        {
            if (_repositories.ContainsKey(typeof(TSet)))
            {
                return _repositories[typeof(TSet)] as IRepository<TSet>;
            }

            var repository = new Repository<TSet>(_dataContext);
            _repositories.Add(typeof(TSet), repository);
            return repository;
        }

    }
}
