namespace UserManagementService.Interfaces.UserTools;

public interface IPasswordHasher
{
    public string GenerateHash(string password);

    public bool VerifyHash(string password, string hash);
}