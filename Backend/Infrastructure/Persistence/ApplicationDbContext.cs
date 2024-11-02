using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<Product>(

                    entity =>
                    {
                        entity.ToTable("products");
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Id)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .ValueGeneratedOnAdd();
                        entity.Property(entity => entity.Type).IsRequired().HasMaxLength(50);
                        entity.Property(entity => entity.Name).IsRequired().HasMaxLength(100);
                        entity.Property(entity => entity.Description).IsRequired();
                        entity.Property(entity => entity.Price).IsRequired();
                        entity.Property(entity => entity.Review).IsRequired();
                    }
                );
        }
    }
}
