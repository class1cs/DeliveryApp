using DeliveryApp.DAL;

namespace DeliveryApp.API.Dtos;

public class EditCourierDto
{
    public string Name { get; set; }
    
    public string SecondName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Role Role { get; set; }
    
    public string Password { get; set; }
    
}