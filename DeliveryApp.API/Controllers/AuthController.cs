using System.Security.Authentication;
using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using DeliveryApp.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginDto = DeliveryApp.API.Dtos.LoginDto;

namespace DeliveryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly ApplicationContext _appContext;
        private readonly RegisterService _registerService;
        private readonly ValidationService _validationService;

        public AuthController
            (
            ApplicationContext appContext, 
            RegisterService registerService, 
            LoginService loginService,
            ValidationService validationService)
        {
            _appContext = appContext;
            _registerService = registerService;
            _loginService = loginService;
            _validationService = validationService;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _loginService.LoginAsync(loginDto);
                return Ok(token);
            }
            catch (InvalidCredentialException e)
            {
                return Conflict(e.Message);
            }
        }
        

        
        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                var token = await _registerService.RegisterAsync(registerUserDto);
                return Ok(token);
            }
            catch (InvalidCredentialException e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
