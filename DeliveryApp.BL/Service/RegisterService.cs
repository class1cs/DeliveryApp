using DeliveryApp.DAL;

namespace DeliveryApp.BL;

public class RegisterService
{
    private readonly ApplicationContext _appContext;

    private readonly IPasswordHasher _passwordHasherService;

    public RegisterService(IPasswordHasher passwordHasherService, ApplicationContext appContext)
    {
        _passwordHasherService = passwordHasherService;
        _appContext = appContext;
    }

    public async Task RegisterAsync()
    {
        //TODO Сделать регистрацию
    }
}