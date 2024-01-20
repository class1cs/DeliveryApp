using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.BL;

public class RegisterService
{
    private readonly ApplicationContext _appContext;

    private readonly IPasswordHasher _passwordHasherService;
    private readonly ValidationService _validationService;
    private readonly TokenService _tokenService;

    public RegisterService(IPasswordHasher passwordHasherService, ApplicationContext appContext, ValidationService validationService, TokenService tokenService)
    {
        _passwordHasherService = passwordHasherService;
        _appContext = appContext;
        _validationService = validationService;
        _tokenService = tokenService;
    }

    public async Task<string> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var hash = _passwordHasherService.HashPassword(registerUserDto.Password);
        var userToAdd = new User()
        {
            Id = new Guid(),
            Name = registerUserDto.Name,
            SecondName = registerUserDto.SecondName,
            Patronymic = registerUserDto.Patronymic,
            PasswordHash = hash,
            Role = Role.User,
            Login = registerUserDto.Login
        };
        if (!_validationService.CheckPasswordMatch(registerUserDto.Password, registerUserDto.ConfirmPassword))
        {
            throw new InvalidCredentialException("Пароли не совпадают!");
        }
        if (await _validationService.CheckExistUserAsync(userToAdd.Login, registerUserDto.Password))
        {
            throw new InvalidCredentialException("Этот аккаунт уже существует!");
        }
        await _appContext.Users.AddAsync(userToAdd);
        await _appContext.SaveChangesAsync();
        return await _tokenService.GenerateTokenAsync(userToAdd);
    }
}