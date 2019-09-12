using Msi.AspNetCore.UnitOfWork;

namespace AspNetCore.UnitOfWork.Example.Data
{
    public class TodoDatabaseOptions : IConnectionStringOptions
    {
        public string ConnectionString { get; set; }
    }
}
