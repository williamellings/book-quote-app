using Microsoft.EntityFrameworkCore;
using BookQuoteApp.Api.Models;

namespace BookQuoteApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Quote> Quotes => Set<Quote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            // Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Description).HasMaxLength(1000);
                entity.Property(b => b.Genre).HasMaxLength(50);

                entity.HasOne(b => b.User)
                      .WithMany(u => u.Books)
                      .HasForeignKey(b => b.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Quote entity
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Text).IsRequired().HasMaxLength(1000);
                entity.Property(q => q.Author).IsRequired().HasMaxLength(100);
                entity.Property(q => q.Category).HasMaxLength(50);

                entity.HasOne(q => q.User)
                      .WithMany(u => u.Quotes)
                      .HasForeignKey(q => q.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
