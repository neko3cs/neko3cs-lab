using Microsoft.EntityFrameworkCore;

namespace DotNetOrmBench;

internal class SampleDbContext(
    string connectionString
) : DbContext
{

    public DbSet<Person> Person { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
}