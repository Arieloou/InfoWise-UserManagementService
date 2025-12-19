using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using UserManagementService.Application.UserTools;
using UserManagementService.Infraestructure.JWT;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Interfaces.UserTools;
using UserManagementService.Models;

namespace UserManagementService.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTResponseGenerator _jWTResponseGenerator;
        
        public UserRepository(ApplicationDBContext context, IPasswordHasher passwordHasher, IJWTResponseGenerator jWTResponseGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jWTResponseGenerator = jWTResponseGenerator;
        }

        public async Task<JWTResponse> Login(User user)
        {
            var userInDB = await _context.Users
                .FirstAsync(u => u.Email == user.Email);

            if (!_passwordHasher.VerifyHash(user.Password!, userInDB.PasswordHash!))
            {
                throw new Exception("The user and/or password are incorrect.");
            }

            return _jWTResponseGenerator.Generate(user!.Email, "client", userInDB.Id.ToString());
        }

        public async Task<JWTResponse> Register(User user)
        {
            user!.PasswordHash = _passwordHasher.GenerateHash(user.Password!);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var userInDB = await _context.Users
                .FirstAsync(u => u.Email == user.Email);

            return _jWTResponseGenerator.Generate(user.Email, "client", userInDB.Id.ToString());
        }
    }
}
