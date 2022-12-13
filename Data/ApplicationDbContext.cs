using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FinalLaboIV.Models;

namespace FinalLaboIV.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierProduct>().HasKey(sc => new { sc.ProductId, sc.SupplierId });

            modelBuilder.Entity<Product>().HasMany(s => s.SupplierProduct);
            modelBuilder.Entity<Supplier>().HasMany(p => p.SupplierProduct);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SupplierProduct> SuppliersProducts { get; set; }
    }
}
