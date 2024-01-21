using DeliveryApp.DAL;

namespace DeliveryApp.API.Dtos;

public class UserResponseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string SecondName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string PasswordHash { get; set; }

    public List<OrderResponseDto> Orders { get; set; } = new();
    
    public string Login { get; set; }
    
    public Role Role { get; set; }
    
}