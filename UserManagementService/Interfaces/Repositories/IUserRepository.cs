using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<JwtResponse> Login(User user);
        public Task<JwtResponse> Register(User user);
        public Task<User?> GetUserById(int id);
        public Task<JwtResponse> RefreshToken(string refreshToken);
        public Task SavePreferences(int userId, int[] categoryIds);
    }
}
