using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.BL;

public class UserService
{
    private readonly ApplicationContext _applicationContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ValidationService _validationService;
    
    public UserService(ApplicationContext applicationContext, IPasswordHasher passwordHasher, ValidationService validationService)
    {
        _applicationContext = applicationContext;
        _passwordHasher = passwordHasher;
        _validationService = validationService;
    }
    public async Task AddUserAsync(AddUserDto addUserDto)
    {
        if (await _validationService.CheckExistUserAsync(addUserDto.Login, addUserDto.Password) == false)
        {
            throw new InvalidCredentialException("Этот пользователь уже существует");
        }
        await _applicationContext.Users.AddAsync(new User
        {
            PasswordHash = _passwordHasher.HashPassword(addUserDto.Password),
            Name = addUserDto.Name,
            SecondName = addUserDto.SecondName,
            Patronymic = addUserDto.Patronymic,
            Role = addUserDto.Role
        });
        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task EditUserAsync(Guid id, EditUserDto editUserDto)
    {       
        var userToEdit = await _applicationContext.Users.FindAsync(id);
        var hash = _passwordHasher.HashPassword(editUserDto.Password);
        
        if (await _applicationContext.Users.Where(x => x.Login != userToEdit.Login && x.PasswordHash != userToEdit.PasswordHash).AnyAsync(x => x.Login == editUserDto.Login && x.PasswordHash == hash))
        {
            throw new InvalidCredentialException("Этот пароль или логин уже занят другим курьером.");
        }
        userToEdit.Name = editUserDto.Name;
        userToEdit.SecondName = editUserDto.SecondName;
        userToEdit.Patronymic = editUserDto.Patronymic;
        userToEdit.Role = editUserDto.Role;
        userToEdit.Login = editUserDto.Login;
        userToEdit.PasswordHash = hash;
        
        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task RemoveUserAsync(Guid id)
    {       
        var userToDelete = await _applicationContext.Users.FindAsync(id);
        _applicationContext.Users.Remove(userToDelete);
        await _applicationContext.SaveChangesAsync();
    }
}