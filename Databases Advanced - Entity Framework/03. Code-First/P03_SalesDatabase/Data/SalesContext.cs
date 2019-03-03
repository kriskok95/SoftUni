using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext() { }

        public SalesContext(DbContextOptions options)
            : base(options) { }        

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.CustomerId);

            modelBuilder.Entity<Customer>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder.Entity<Customer>()
                .Property(x => x.Email)
                .HasMaxLength(80)
                .IsUnicode();

            modelBuilder.Entity<Product>()
                .HasKey(x => x.ProductId);

            modelBuilder.Entity<Product>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder.Entity<Product>()
                .Property(x => x.Description)
                .HasMaxLength(250)
                .HasDefaultValue("No description");

            modelBuilder.Entity<Store>()
                .HasKey(x => x.StoreId);

            modelBuilder.Entity<Store>()
                .Property(x => x.Name)
                .HasMaxLength(80)
                .IsUnicode();

            modelBuilder.Entity<Sale>()
                .HasKey(x => x.SaleId);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .HasMany(x => x.Sales)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Customer>()
                .HasMany(x => x.Sales)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);
    
        }
    }
}
