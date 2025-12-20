using Microsoft.AspNetCore.Mvc;
using UserManagementService.Application.Services;
using UserManagementService.Interfaces.Repositories;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class UsersController : Controller
    {
        private readonly UserAppService _service;

        public UsersController(UserAppService service)
        {
            _service = service;
        }
        
        [HttpPost("{userId}/preferences")]
        public async Task<IActionResult> SetPreferences(Guid userId, [FromBody] int[] categoryIds)
        {
            await _service.SetUserPreferences(userId, categoryIds);

            return Ok();
        }
    }    
}
