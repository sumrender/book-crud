using Microsoft.EntityFrameworkCore;
using BooksCrudApi.Models;

namespace BooksCrudApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                
                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.ISBN)
                    .HasMaxLength(50);
                
                entity.Property(e => e.Publisher)
                    .HasMaxLength(50);
                
                entity.Property(e => e.Genre)
                    .HasMaxLength(50);
                
                entity.Property(e => e.Language)
                    .HasMaxLength(20);
                
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");
                
                entity.Property(e => e.CoverImageUrl)
                    .HasMaxLength(500);
                
                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("GETUTCDATE()");
                
                entity.Property(e => e.UpdatedOn);
            });

        }
    }
} 