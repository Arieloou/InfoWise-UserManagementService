namespace UserManagementService.Infrastructure.DTOs;

public class UserResponseDto
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public List<int>? PreferredCategoryIds { get; set; }
}