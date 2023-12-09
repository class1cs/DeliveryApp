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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasData(new List<Role>
            {
                new()
                {
                    Name = "User",
                    Id = Guid.Parse("1e663e91-3548-42b7-97d1-b886ec5b0a41")
                },
                new()
                {
                    Name = "Courier",
                    Id = Guid.Parse("0c48d6e0-4405-4afe-9efe-76448bc4466e")
                },
                new()
                {
                    Name = "Admin",
                    Id = Guid.Parse("9b6d7abb-41f5-4223-81e9-430729e41618")
                }
            });
        base.OnModelCreating(modelBuilder);
    }
}