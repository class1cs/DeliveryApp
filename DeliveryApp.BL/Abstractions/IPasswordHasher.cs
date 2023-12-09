namespace DeliveryApp.BL;

public interface IPasswordHasher
{
    string HashPassword(string password);
}