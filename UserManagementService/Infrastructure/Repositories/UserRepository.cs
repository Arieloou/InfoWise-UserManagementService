using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using UserManagementService.Application.UserTools;
using UserManagementService.Infrastructure.JWT;
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
        public async Task<JwtResponse> Login(User user)
        {
            var userInDb = await context.Users
                .FirstAsync(u => u.Email == user.Email);

            return !passwordHasher.VerifyHash(user.Password!, userInDb.PasswordHash!) ? throw new Exception("The user and/or password are incorrect.") : jWtResponseGenerator.Generate(user!.Email, "client", userInDb.Id.ToString());
        }

        public async Task<JwtResponse> Register(User user)
        {
            user!.PasswordHash = passwordHasher.GenerateHash(user.Password!);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userInDb = await context.Users
                .FirstAsync(u => u.Email == user.Email);

            return jWtResponseGenerator.Generate(user.Email, "client", userInDb.Id.ToString());
        }

        public async Task<User?> GetUserById(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public Task<JwtResponse> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task SavePreferences(int userId, int[] categoryIds)
        {
            var existingPreferences = await context.UserPreferences
                .Where(up => up.UserId == userId)
                .ToListAsync();
            
            // We delete the existing user preference to reset all of them
            if (existingPreferences.Any())
            {
                context.UserPreferences.RemoveRange(existingPreferences);
            }
            
            var newPreferences = categoryIds.Select(categoryId => new UserPreference
            {
                UserId = userId,
                SubscribedCategoryId = categoryId
            }).ToList();

            await context.UserPreferences.AddRangeAsync(newPreferences);
            
            await context.SaveChangesAsync();
        }
    }
}
