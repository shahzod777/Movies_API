using Microsoft.EntityFrameworkCore;
using movies.Entities;

namespace movies.Data
{
    public class MoviesContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Actor> Actors { get; set; }
        
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Image> Images { get; set; }
        
        public MoviesContext(DbContextOptions options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}