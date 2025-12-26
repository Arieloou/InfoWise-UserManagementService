namespace UserManagementService.Infrastructure.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<int> PreferredCategoryIds { get; set; }
}