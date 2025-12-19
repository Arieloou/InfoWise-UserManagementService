namespace UserManagementService.Interfaces.JWT
{
    public interface IJWTGenerator
    {
        public string GenerateToken(string email, string role, string userId);
    }
}
