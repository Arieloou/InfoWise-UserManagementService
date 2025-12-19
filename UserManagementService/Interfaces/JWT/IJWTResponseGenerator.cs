using UserManagementService.Models;

namespace UserManagementService.Interfaces.UserTools
{
    public interface IJWTResponseGenerator
    {
        public JWTResponse Generate(string email, string role);

    }
}
