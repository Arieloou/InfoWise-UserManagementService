using UserManagementService.Interfaces.JWT;
using UserManagementService.Interfaces.UserTools;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure.JWT
{
    public class JwtResponseGenerator : IJWTResponseGenerator
    {
        private readonly IJWTGenerator _jWTGenerator;

        public JwtResponseGenerator(IJWTGenerator jWTGenerator)
        {
            _jWTGenerator = jWTGenerator;
        }

        public JwtResponse Generate(string email, string role, string userId)
        {
            return new JwtResponse
            {
                Token = _jWTGenerator.GenerateToken(email, role, userId),
                Email = email,
                Role = role,
                UserId = userId
            };
        }
    }
}
