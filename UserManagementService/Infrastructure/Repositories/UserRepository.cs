using Microsoft.EntityFrameworkCore;
using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Interfaces.UserTools;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure.Repositories
{
    public class UserRepository(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJWTResponseGenerator jWtResponseGenerator)
        : IUserRepository
    {
        public async Task<JwtResponse> Login(UserDto userDto)
        {
            var userInDb = await context.Users  
                .FirstAsync(u => u.Email == userDto.Email);

            return !passwordHasher.VerifyHash(userDto.Password!, userInDb.PasswordHash!) ? throw new Exception("The user and/or password are incorrect.") : jWtResponseGenerator.Generate(userDto!.Email, "client", userInDb.Id.ToString());
        }

        public async Task<JwtResponse> Register(UserDto userDto)
        {
            var passwordHash = passwordHasher.GenerateHash(userDto.Password!);
        
            // We create the user with the information from the body
            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = passwordHash,
                Password =  userDto.Password,
            };
            
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userInDb = await context.Users
                .FirstAsync(u => u.Email == userDto.Email);

            return jWtResponseGenerator.Generate(userDto.Email, "client", userInDb.Id.ToString());
        }

        public async Task<User?> GetUserById(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public Task<JwtResponse> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
