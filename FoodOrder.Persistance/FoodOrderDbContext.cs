using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FoodOrder.Persistance
{
    public class FoodOrderDbContext : IdentityDbContext<Employees, IdentityRole<int>, int>
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<Employees> Employees { get; set; } = null!;
        public DbSet<Orders> Orders { get; set; } = null!;
        public DbSet<ProductOrder> ProductOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employees>().ToTable("Employees");
        }

        public FoodOrderDbContext(DbContextOptions<FoodOrderDbContext> options) : base(options)
        {
        }
    }
}
