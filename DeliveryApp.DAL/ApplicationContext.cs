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
        var adminRole = new Role()
        {
            Name = "Admin", 
            Id = Guid.Parse("9b6d7abb-41f5-4223-81e9-430729e41618")
        };
        var userRole = new Role()
        { 
            Name = "User",
            Id = Guid.Parse("1e663e91-3548-42b7-97d1-b886ec5b0a41")
        };
        var courierRole = new Role()
        {
            Name = "Courier",
            Id = Guid.Parse("0c48d6e0-4405-4afe-9efe-76448bc4466e")
        };
        modelBuilder.Entity<Role>()
            .HasData(new List<Role>
            {
               userRole,
               courierRole,
                adminRole
            });
        modelBuilder.Entity<User>()
            .HasData(new List<User>
            {
                new User
                {
                    Login = "admin", 
                    PasswordHash = "ISMvKXpXpadDiUoOSoAfww==", 
                    Name = "Алексей", 
                    SecondName = "Макаров", 
                    Patronymic = "Валерьевич", 
                    PhoneNumber = "+79125126131", 
                    Role = adminRole
                }
            });
        base.OnModelCreating(modelBuilder);
    }
}