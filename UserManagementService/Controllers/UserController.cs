using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Application.Services;
using UserManagementService.Infrastructure.DTOs;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(UserAppService service, ILogger<UserController> logger) : Controller
    {
        [HttpGet("{userId}/preferences")]
        public async Task<IActionResult> GetPreferences(int userId)
        {
            try
            {
                var preferences = await service.GetUserPreferences(userId);
                return Ok(preferences);
            }
            catch
            {
                return Conflict("The user doesn't have any preference configured.");
            }
        }
        
        [HttpPost("preferences/upsert")]
        public async Task<IActionResult> SetPreferences([FromBody] UserPreferencesDto userPreferencesDto)
        {
            try
            {
                await service.SetUserPreferences(userPreferencesDto.UserId, userPreferencesDto.CategoryIds);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Internal error: " + e.Message);
            }
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto? userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                return Unauthorized("User is empty.");
            }

            try
            {
                var jwtResponse = await service.RegisterUser(userDto);
                return Ok(jwtResponse);
            }
            catch (DbUpdateException ex) 
            {
                // Postgres code for unique foreign key violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("23505")) 
                {
                    return Conflict("The user already exists.");
                }
                
                logger.LogError(ex, "Error trying to register a new user");
                return StatusCode(500, "Internal Server Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserDto? userDto)
        {
            if (userDto?.Password == null)
            {
                return Unauthorized("User is empty.");
            }

            try
            {
                var jwtResponse = await service.LoginUser(userDto);
                return Ok(jwtResponse);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return Unauthorized("The user and/or password are incorrect.");
            }
        }
    }    
}
