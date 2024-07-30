using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Pos_System_3.Model;

namespace Pos_System_3.Data
{
    public class DBContextEntity : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DBContextEntity(DbContextOptions<DBContextEntity> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the decimal precision for Price
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductItem>()
                .Property(pi => pi.Price)
                .HasColumnType("decimal(18,2)");

            // Configure User - Sale relationship
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Cashier)
                .WithMany()
                .HasForeignKey(s => s.CashierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Sale - ProductItem relationship
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Products)
                .WithOne(pi => pi.Sale)
                .HasForeignKey(pi => pi.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ProductItem - Product relationship
            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductItems)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Product - Category relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure primary key for ProductItem
            modelBuilder.Entity<ProductItem>()
                .HasKey(pi => new { pi.SaleId, pi.ProductId });

            // Configure relationships in ProductItem
            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Sale)
                .WithMany(s => s.Products)
                .HasForeignKey(pi => pi.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductItems)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Purchase - ProductItem relationship
            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.Products)
                .WithOne(pi => pi.Purchase)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("InMemoryDb");
            }
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(
        //            "Server=INZAMAM-9VR3CJN\\SQLEXPRESS;Database=POSSystem;Trusted_Connection=True;TrustServerCertificate=True;",
        //            options => options.MigrationsAssembly("POSAPIs")
        //        );
        //    }
        //}
    }

}
