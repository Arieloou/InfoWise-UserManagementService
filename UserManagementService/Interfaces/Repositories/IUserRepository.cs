using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<JWTResponse> Login(User user);
        public Task<JWTResponse> Register(User user);
    }
}
