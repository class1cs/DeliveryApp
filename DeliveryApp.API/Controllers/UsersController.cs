using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.BL;
using DeliveryApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly UserService _userService;

        public UsersController(ApplicationContext applicationContext, IPasswordHasher passwordHasher, ValidationService validationService, UserService userService)
        {
            _applicationContext = applicationContext;
            _passwordHasher = passwordHasher;
            _validationService = validationService;
            _userService = userService;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserDto addUserDto)
        {
            try
            {
                await _userService.AddUserAsync(addUserDto);
                return Ok("Пользователь успешно добавлен!");
            }
            catch (InvalidCredentialException e)
            {
                return Conflict(e.Message);
            }
            
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                await _userService.RemoveUserAsync(id);
                return Ok("Пользователь успешно удален!");
            }
            catch (InvalidCredentialException e)
            {
                return Conflict(e.Message);
            }
            
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditUserDto editUserDto)
        {
            try
            {
                await _userService.EditUserAsync(id, editUserDto);
                return Ok("Пользователь успешно отредактирован!");
            }
            catch (InvalidCredentialException e)
            {
                return Conflict(e.Message);
            }
            
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _applicationContext.Users.Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == id);
           return Ok(user);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(Role? role = null)
        {
            if (role == null)
            {
                var allUsers = await _applicationContext.Users.Include(x => x.Orders).ThenInclude(x => x.Items).ToListAsync();
                return Ok(allUsers);
            }
            var users = await _applicationContext.Users.Include(x => x.Orders).ThenInclude(x => x.Items).Where(x => x.Role == role).ToListAsync();
            return Ok(users);
        }
    }
}
