using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst.Entity;

public class DatabaseContext(string connectionString) : DbContext
{
  public readonly string _connectionString = connectionString;

  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseSqlServer(_connectionString);
}
