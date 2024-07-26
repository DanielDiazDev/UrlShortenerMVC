using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlMapping>(entity =>
            {
                entity.HasKey(u => u.Id);
                
                entity.Property(u => u.OriginalUrl)
               .HasMaxLength(2048)
               .IsRequired();

                entity.Property(u => u.ShortUrl)
                       .HasMaxLength(256)
                       .IsRequired();

                entity.Property(u => u.CreatedAt)
                       .HasColumnType("datetime2");

                entity.Property(u => u.ExpirationDate)
                       .HasColumnType("datetime2");

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserName)
                .HasMaxLength(256)
                .IsRequired();

                entity.Property(u => u.Email)
                       .HasMaxLength(256)
                       .IsRequired();

                entity.Property(u => u.Password)
                       .HasMaxLength(256)
                       .IsRequired();

                entity.HasMany(u => u.UrlMappings)
                       .WithOne()
                       .HasForeignKey(u=>u.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UrlMapping> UrlMappings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
