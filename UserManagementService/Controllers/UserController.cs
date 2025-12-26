using Microsoft.AspNetCore.Mvc;
using UserManagementService.Application.Services;
using UserManagementService.Models;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(UserAppService service, ILogger<UserController> logger) : Controller
    {
        [HttpPost("{userId}/preferences")]
        public async Task<IActionResult> SetPreferences(int userId, [FromBody] int[] categoryIds)
        {
            await service.SetUserPreferences(userId, categoryIds);
            return Ok();
        }

        [HttpGet("{userId}/preferences")]
        public async Task<IActionResult> GetPreferences(int userId)
        {
            var preferences = await service.GetUserPreferences(userId);
            return Ok(preferences);
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] User? user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return Unauthorized("User is empty.");
            }

            try
            {
                var response = await service.RegisterUser(user);
                return Ok(response);
            }
            catch
            {
                return Conflict("The user already exists.");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User? user)
        {
            if (user?.Password == null)
            {
                return Unauthorized("User is empty.");
            }

            try
            {
                var response = await service.LoginUser(user);
                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return Unauthorized("The user and/or password are incorrect.");
            }
        }
    }    
}
