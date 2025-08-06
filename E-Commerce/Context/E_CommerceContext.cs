using E_Commerce.Helper;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace E_Commerce.Context
{
    public class E_CommerceContext : DbContext
    {
        public E_CommerceContext(DbContextOptions<E_CommerceContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductMedia> ProductMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Description).HasDefaultValue("");
                entity.Property(e => e.Is_Deleted).HasDefaultValue(false);
                entity.Property(e => e.Created_At).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasDefaultValue("");
                entity.Property(e => e.Is_Deleted).HasDefaultValue(false);
                entity.Property(e => e.Created_At).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasData(new User
                {
                    Id = 1,
                    Username= "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = "wcIksDzZvHtqhtd/XazkAZF2bEhc1V3EjK+ayHMzXW8=",
                    Role = Role.Admin,
                    Created_At = new DateTime(2025, 8, 5, 12, 27, 53),
                    Is_Deleted = false
                 });

                entity.Property(e => e.Is_Deleted).HasDefaultValue(false);
                entity.Property(e => e.Created_At).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<ProductMedia>(entity =>
            {
                entity.Property(e => e.Is_Deleted).HasDefaultValue(false);
                entity.Property(e => e.Created_At).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
