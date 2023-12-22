using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.BL;

public class CourierService
{
    private readonly ApplicationContext _applicationContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ValidationService _validationService;
    
    public CourierService(ApplicationContext applicationContext, IPasswordHasher passwordHasher, ValidationService validationService)
    {
        _applicationContext = applicationContext;
        _passwordHasher = passwordHasher;
        _validationService = validationService;
    }
    public async Task AddCourierAsync(AddCourierDto addCourierDto)
    {
        if (await _validationService.CheckExistUserAsync(addCourierDto.PhoneNumber, addCourierDto.Password) == false)
        {
            throw new InvalidCredentialException("Этот курьер уже существует");
        }
        await _applicationContext.Users.AddAsync(new User
        {
            PasswordHash = _passwordHasher.HashPassword(addCourierDto.Password),
            Name = addCourierDto.Name,
            SecondName = addCourierDto.SecondName,
            Patronymic = addCourierDto.Patronymic,
            PhoneNumber = addCourierDto.PhoneNumber, 
            Role = await _applicationContext.Roles.FirstOrDefaultAsync(x => x.Name == "Courier")
        });
        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task EditCourierAsync(Guid id, EditCourierDto editCourierDto)
    {       
            var userToEdit = await _applicationContext.Users.FindAsync(id);
            var hash = _passwordHasher.HashPassword(editCourierDto.Password);
            if (await _applicationContext.Users.AnyAsync(x =>
                    x.PhoneNumber == editCourierDto.PhoneNumber && x.PasswordHash == hash) == false)
            {
                throw new InvalidCredentialException("Этот пароль или номер телефона уже занят другим курьером.");
            }
            userToEdit.Name = editCourierDto.Name;
            userToEdit.SecondName = editCourierDto.SecondName;
            userToEdit.Patronymic = editCourierDto.Patronymic;
            userToEdit.Role = editCourierDto.Role;
            userToEdit.PhoneNumber = editCourierDto.PhoneNumber;
            await _applicationContext.SaveChangesAsync();
    }
    
    public async Task RemoveCourierAsync(Guid id)
    {       
        var userToDelete = await _applicationContext.Users.FindAsync(id);
        _applicationContext.Users.Remove(userToDelete);
        await _applicationContext.SaveChangesAsync();
    }
}