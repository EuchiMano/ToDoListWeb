using Microsoft.EntityFrameworkCore;

namespace ToDoListWebApi.Data;

public class ToDoContext : DbContext
{
  public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
  {

  }

  public DbSet<Models.Task> Tasks { get; set; }
}
