using DeliveryApp.API.Dtos;
using DeliveryApp.BL;
using DeliveryApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.API.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ValidationService _validationService;

        public UsersController(ApplicationContext applicationContext, IPasswordHasher passwordHasher, ValidationService validationService)
        {
            _applicationContext = applicationContext;
            _passwordHasher = passwordHasher;
            _validationService = validationService;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserDto addUserDto)
        {
            if (await _validationService.CheckExistUserAsync(addUserDto.Login, addUserDto.Password) == false)
            {
                return Conflict("Этот пользователь уже существует");
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
            return Ok("Пользователь был успешно добавлен!");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                var userToDelete = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (userToDelete == null)
                {
                    return NotFound("Пользователь не найден!");
                }
                _applicationContext.Users.Remove(userToDelete);
                await _applicationContext.SaveChangesAsync();
                return NotFound("Пользователь успешно удален.");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("Произошла неизвестная ошибка.");
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditUserDto editUserDto)
        {
            var userToEdit = await _applicationContext.Users.FindAsync(id);
            var hash = _passwordHasher.HashPassword(editUserDto.Password);
            var users = _applicationContext.Users.Where(x =>
                x.Login != userToEdit.Login && x.PasswordHash != userToEdit.PasswordHash);
            if (await users.AnyAsync(x => x.Login == editUserDto.Login && x.PasswordHash == hash))
            {
               return Conflict("Этот пароль или логин уже занят другим курьером.");
            }
            userToEdit.Name = editUserDto.Name;
            userToEdit.SecondName = editUserDto.SecondName;
            userToEdit.Patronymic = editUserDto.Patronymic;
            userToEdit.Role = editUserDto.Role;
            userToEdit.Login = editUserDto.Login;
            userToEdit.PasswordHash = hash;
            await _applicationContext.SaveChangesAsync();
            return Ok("Пользователь был успешно изменен.");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _applicationContext.Users.Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(user);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUsers = await _applicationContext
                .Users
                .AsNoTracking()
                .Include(x => x.Orders)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Product).AsSplitQuery().ToListAsync();
            
            var allUsersDtos = allUsers.Select(x => new UserResponseDto
            {
                Id = x.Id, 
                Login = x.Login,
                Orders = x.Orders.Select(x => new OrderResponseDto
                {
                    DeliveryDate = x.DeliveryDate, 
                    Id = x.Id,
                    Items = x.Items, 
                    Requests = x.Requests,
                    Status = x.Status
                }).ToList(),
                Name = x.Name, PasswordHash = x.PasswordHash, Patronymic = x.Patronymic, Role = x.Role,
                SecondName = x.SecondName
            });
            return Ok(allUsersDtos);
        }
    }
}
