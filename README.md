EFCore.Data.Repository
======================

Repository written in C# for EntityFrameworkCore.

I can guess that you are encountering more than one repository. You can simply use the repository I have prepared in your projects. You can use directly generic repository while injecting in ctor. Since it's already generic, you only need to provide an entity. Let's setup it together.


### How to setup?

##### 1. Creating Entity

```csharp
    public class User
    {
        public Guid Identifier { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
```

##### 2.Creating Basic DbContext

```csharp
public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Replace with your connection string.
        var connectionString = _configuration.GetConnectionString("TestDbConnection");

        options.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
    }
}
```

##### 3.Creating Controller and Injecting Repository

```csharp
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _repository;

    public UserController(IRepository<User> repository)
    {
        _repository = repository;
    }
}
```

##### 4.Creating Endpoint and Using Repository with CRUD Operation

```csharp
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _repository;

    public UserController(IRepository<User> repository)
    {
        _repository = repository;
    }

        #region Read

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Get list of data
            var data = await _repository.GetListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Get data by id
            var data = await _repository.GetFirstOrDefaultAsync(x => x.Identifier == id);

            return Ok(data);
        }

        #endregion

        #region Create

        [HttpPost]
        public async Task<IActionResult> PostAsync(User user)
        {
            // Create
            await _repository.AddAsync(user);

            return Ok();
        }

        #endregion

        #region Update

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(User user)
        {
            // Get entity
            var entity = await _repository.GetFirstOrDefaultAsync(x => x.Identifier.Equals(user.Identifier));

            // Set new values
            entity.Username = user.Username;
            entity.Email = user.Email;
            entity.PasswordHash = user.PasswordHash;

            // Update
            await _repository.UpdateAsync(user);

            return NoContent();
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Get entity
            var entity = await _repository.GetFirstOrDefaultAsync(x => x.Identifier.Equals(id));

            // Delete
            await _repository.DeleteAsync(entity);

            return Ok();
        }

        #endregion
}
```

##### 5.DI container Registration
```csharp
...
services.AddTransient<IRepository<User>, Repository<ApplicationDbContext, User>>();
...
```

Finally we did it. It will be continued to develop. You can fork. 
I will make a youtube video.