using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<JwtResponse> Login(User user);
        public Task<JwtResponse> Register(User user);
        public Task<User?> GetUserById(Guid id);
        public Task<JwtResponse> RefreshToken(string refreshToken);
        public Task SavePreferences(Guid userId, int[] categoryIds);
    }
}
