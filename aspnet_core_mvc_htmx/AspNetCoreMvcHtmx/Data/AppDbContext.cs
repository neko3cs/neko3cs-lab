using Microsoft.EntityFrameworkCore;
using AspNetCoreMvcHtmx.Models;

namespace AspNetCoreMvcHtmx.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Prefecture> Prefectures { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prefecture>()
                .HasMany(p => p.Cities)
                .WithOne(c => c.Prefecture)
                .HasForeignKey(c => c.PrefectureId);
        }
    }
}
