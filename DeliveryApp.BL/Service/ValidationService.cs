using DeliveryApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.BL;

public class ValidationService
{
    private readonly ApplicationContext _appContext;

    private readonly IPasswordHasher _passwordHasherService;

    public ValidationService(ApplicationContext applicationContext,
        IPasswordHasher passwordHasherService)
    {
        _appContext = applicationContext;
        _passwordHasherService = passwordHasherService;
    }

    public Task<bool> CheckExistUserAsync(string login, string password)
    {
        var hash = _passwordHasherService.HashPassword(password);
        return _appContext.Users.AnyAsync(x =>
            x.Login == login && x.PasswordHash == hash);
    }
    
}
        
        
       
        