using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.DAL;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        
    }
}