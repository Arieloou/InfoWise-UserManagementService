using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<JwtResponse> Login(UserDto userDto);
        public Task<JwtResponse> Register(UserDto userDto);
        public Task<User?> GetUserById(int id);
        public Task<JwtResponse> RefreshToken(string refreshToken);
    }
}
