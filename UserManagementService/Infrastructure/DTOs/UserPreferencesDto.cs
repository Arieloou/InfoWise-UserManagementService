namespace UserManagementService.Infrastructure.DTOs;

public class UserPreferencesDto
{
    public required int UserId { get; set; }
    public List<int>? CategoryIds { get; set; }
}