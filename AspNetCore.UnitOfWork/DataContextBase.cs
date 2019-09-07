using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCore.UnitOfWork
{
    public class DataContextBase<TContext> : DbContext, IDataContext where TContext : DbContext
    {

        public DataContextBase()
        {
        }

        public DataContextBase(DbContextOptions<TContext> options) : base(options)
        {
        }

        public IEnumerable<EntityEntry> GetChangeTrackerEntries()
        {
            return ChangeTracker.Entries();
        }

    }
}
