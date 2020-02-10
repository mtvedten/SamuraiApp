using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext

    {

   

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
              //  .UseLoggerFactory(MyConsoleLoggerFactory)
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SamuraiApp;Trusted_Connection=True;MultipleActiveResultSets=true");
         
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });
        }
    }
}
