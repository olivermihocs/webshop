using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class DrinkStoreContext : IdentityDbContext<Employee, IdentityRole<int>, int>
    {
        public DrinkStoreContext(DbContextOptions<DrinkStoreContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<OrderLine>().HasKey(l => new { l.OrderId, l.ProductId });
        }

        //Főkategóriák
        public DbSet<MainCategory> MainCategories { get; set; }

        //Alkategóriák
        public DbSet<Category> Categories { get; set; }

        //Termékek
        public DbSet<Product> Products { get; set; }

        //Rendelések
        public DbSet<Order> Orders { get; set; }

        //Rendelés sorok
        public DbSet<OrderLine> OrderLines { get; set; }

    }
}
