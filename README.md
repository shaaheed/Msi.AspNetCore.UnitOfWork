# Msi.AspNetCore.UnitOfWork
UnitOfWork, Repository pattern implementation for ASP.NET Core data accesss layer.

## How to use UnitOfWork

### Create DbContext

```csharp

public class TodoDbContext : DataContextBase<TodoDbContext>
{
    
    public TodoDbContext() : base() { }
    
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
    
    public DbSet<Todo> Todos { get; set; }
}

```

### Configure UnitOfWork

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddUnitOfWork<TodoDbContext, TodoDatabaseOptions>();
}

```

### Use UnitOfWork
```csharp

private readonly IUnitOfWork<TodoDbContext> _unitOfWork;
private readonly IRepository<Todo> _todoRepository;

public ToDosController(IUnitOfWork<TodoDbContext> unitOfWork)
{
    _unitOfWork = unitOfWork;
    _todoRepository = _unitOfWork.GetRepository<Todo>();
}

[HttpGet]
public async Task<IActionResult> GetTodos()
{
    var result = await _todoRepository.AsQueryable().ToListAsync();
    return Ok(result);
}

```
