using Microsoft.EntityFrameworkCore;

namespace InteractivePresentation.Domain.Entity
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(
                "Data Source=presentations.db"
                , b => b.MigrationsAssembly("Api"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Poll>().HasKey(p => p.Id);
            modelBuilder.Entity<Vote>().HasKey(v => v.Id);
            modelBuilder.Entity<Option>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();
        }
    }
}