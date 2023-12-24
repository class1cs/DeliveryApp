namespace DeliveryApp.API.Dtos;

public class RegisterUserDto
{
    public string Login { get; set; }
    
    public string Name { get; set; }
    
    public string SecondName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
}