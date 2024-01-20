using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.DAL;

public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; init; }
    
    public DbSet<Product> Products { get; init; }
        
    public DbSet<Item> Items { get; init; }
    
    public DbSet<Order> Orders { get; init; }
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override  async void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
            .HasData(new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    
                    PasswordHash = "ISMvKXpXpadDiUoOSoAfww==",
                    Name = "Алексей",
                    SecondName = "Макаров",
                    Patronymic = "Валерьевич",
                    Login = "admin",
                
                    Role = Role.Admin
                }
            });
        modelBuilder.Entity<Product>()
            .HasData(new List<Product>
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    
                   Cost = 120,
                   Name = "Ириски 'Золотой октябрь'"
                   
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    
                    Cost = 90,
                    Name = "Добрый cola"
                   
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    
                    Cost = 85,
                    Name = "Тархун"
                   
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    
                    Cost = 55,
                    Name = "Чипсы Lays со вкусом сметаны"
                   
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    
                    Cost = 15,
                    Name = "Сухарики ХрусTeam"
                   
                },
            });
        base.OnModelCreating(modelBuilder);
    }
}