using System.Security.Cryptography;
using System.Text;

namespace DeliveryApp.BL;

public class Md5PasswordHasherService : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var md5 = MD5.Create();
        var computedHash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = Convert.ToBase64String(computedHash);
        return hash;
    }
}