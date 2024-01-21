using DeliveryApp.DAL;

namespace DeliveryApp.API.Dtos;

public class AddUserDto
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string Name { get; set; }
    
    public string SecondName { get; set; }
    
    public Role Role { get; set; }
    
    public string Patronymic { get; set; }
    
}