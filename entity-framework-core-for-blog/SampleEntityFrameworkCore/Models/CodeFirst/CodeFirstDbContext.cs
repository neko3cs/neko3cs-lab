using Microsoft.EntityFrameworkCore;

namespace SampleEntityFrameworkCore.Models.CodeFirst
{
    public class CodeFirstDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=codeFirstDb.db3");
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}