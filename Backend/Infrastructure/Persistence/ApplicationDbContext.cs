using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.HasOne(p => p.User)
                    .WithMany(u => u.Products)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Review).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(30);
                entity.Property(e => e.Password)
                      .IsRequired();
                entity.Property(e => e.Email)
                      .IsRequired();

                // Adăugăm configurarea pentru PhoneNumber (opțional)
                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20)     // sau oricare limită dorești
                      .IsRequired(false);

                entity.HasOne(u => u.Cart)
                      .WithOne(c => c.User)
                      .HasForeignKey<Cart>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("carts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.UserId)
                      .IsRequired();

                entity.HasMany(c => c.Products)
                      .WithMany(p => p.Carts)
                      .UsingEntity(j => j.ToTable("cart_products"));
            });
        }

    }
}
