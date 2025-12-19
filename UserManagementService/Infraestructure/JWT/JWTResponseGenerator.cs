using UserManagementService.Interfaces.JWT;
using UserManagementService.Interfaces.UserTools;
using UserManagementService.Models;

namespace UserManagementService.Infraestructure.JWT
{
    public class JWTResponseGenerator : IJWTResponseGenerator
    {
        private readonly IJWTGenerator _jWTGenerator;

        public JWTResponseGenerator(IJWTGenerator jWTGenerator)
        {
            _jWTGenerator = jWTGenerator;
        }

        public JWTResponse Generate(string email, string role)
        {
            return new JWTResponse
            {
                Token = _jWTGenerator.GenerateToken(email, role),
                Email = email,
                Role = role,
            };
        }
    }
}
