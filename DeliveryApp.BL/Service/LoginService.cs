using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using Microsoft.EntityFrameworkCore;
using LoginDto = DeliveryApp.API.Dtos.LoginDto;

namespace DeliveryApp.BL;

public class LoginService
{
    private readonly ApplicationContext _appContext;

    private readonly IPasswordHasher _passwordHasherService;

    private readonly TokenService _tokenService;

    private readonly ValidationService _validationService;

    public LoginService(IPasswordHasher passwordHasherService, ApplicationContext appContext,
        ValidationService validationService,
        TokenService tokenService)
    {
        _passwordHasherService = passwordHasherService;
        _appContext = appContext;
        _validationService = validationService;
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var passwordHash = _passwordHasherService.HashPassword(loginDto.Password);

        var user = await _appContext.Users
            .FirstOrDefaultAsync(x => x.Login == loginDto.Login && x.PasswordHash == passwordHash);

        if (user is not null)
        {
            return await _tokenService.GenerateTokenAsync(user);
        }

        throw new InvalidCredentialException("Неверные данные.");
    }
    
}
    
