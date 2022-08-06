using System;
using System.Data.Entity;
using TechShop.Entities.Services;

namespace TechShop.Database
{
    public class TSContext : DbContext, IDisposable
    {
        public TSContext() : base("TechShopConnection")
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }
}
