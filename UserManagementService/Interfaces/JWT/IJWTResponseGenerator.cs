using UserManagementService.Models;

namespace UserManagementService.Interfaces.UserTools
{
    public interface IJWTResponseGenerator
    {
        public JwtResponse Generate(string email, string role, string userId);

    }
}
