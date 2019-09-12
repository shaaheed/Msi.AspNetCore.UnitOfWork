using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Msi.AspNetCore.UnitOfWork
{
    public class DataContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private string _connectionString;

        public TContext CreateDbContext(params string[] args)
        {
            if (args.Length > 0)
            {
                _connectionString = args[0];
            }
            return Create(_connectionString);
        }

        public TContext Create(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<TContext>()
                .UseDefaultSqlServer(connectionString, typeof(TContext).GetTypeInfo().Assembly.GetName().Name);

            var context = Activator.CreateInstance(typeof(TContext), new object[] { builder.Options }) as TContext;
            return context;
        }
    }
}
