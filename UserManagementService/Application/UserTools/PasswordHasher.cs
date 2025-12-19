using System.Text;
using UserManagementService.Interfaces.UserTools;

namespace UserManagementService.Application.UserTools
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateHash(string password)
        {
            var buffer = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(buffer);
        }

        public bool VerifyHash(string password, string hash)
        {
            return GenerateHash(password) == hash;
        }
    }
}
