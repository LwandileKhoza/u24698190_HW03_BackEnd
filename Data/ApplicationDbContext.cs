using Microsoft.EntityFrameworkCore;
using FilmLog.API.Models;

namespace FilmLog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }
        public DbSet<WatchedItem> WatchedItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WatchlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.Movie)
                .WithMany(m => m.WatchlistItems)
                .HasForeignKey(w => w.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchedItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WatchedItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchedItem>()
                .HasOne(w => w.Movie)
                .WithMany(m => m.WatchedItems)
                .HasForeignKey(w => w.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
