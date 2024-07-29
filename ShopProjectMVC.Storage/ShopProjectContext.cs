using Microsoft.EntityFrameworkCore;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Storage;
    public class ShopProjectContext:DbContext
    {
        public ShopProjectContext(DbContextOptions<ShopProjectContext> options):
            base(options)
        { 
        }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
    public DbSet<User> Users { get; set; } 
    public DbSet<Product> Product { get; set; } 
    public DbSet<Order> Orders { get; set; } 
    public DbSet<Category> Categories { get; set; } 
    }

