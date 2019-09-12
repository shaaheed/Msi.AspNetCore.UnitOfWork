using AspNetCore.UnitOfWork.Example.Models;
using Microsoft.EntityFrameworkCore;
using Msi.AspNetCore.UnitOfWork;

namespace AspNetCore.UnitOfWork.Example.Data
{
    public class TodoDbContext : DataContextBase<TodoDbContext>
    {

        public TodoDbContext() : base() {
        }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Todo> Todos { get; set; }

    }
}
