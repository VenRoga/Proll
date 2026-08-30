using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Proll.Shared.BaseModels;
using Proll.Api.Models.BaseModels;

namespace Proll.Api.Models.BaseModelsContext
{
    public class BaseModelContext : DbContext
    {
        public BaseModelContext(DbContextOptions<BaseModelContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set;  }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasData(Product.GetSeedData());
        }

    }
}
