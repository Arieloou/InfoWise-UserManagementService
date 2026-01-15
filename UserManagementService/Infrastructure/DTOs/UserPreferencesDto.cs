namespace UserManagementService.Infrastructure.DTOs;

public class UserPreferencesDto
{
    public required int UserId { get; set; }
    public int ShippingHour { get; set; }
    public required List<int> CategoryIds { get; set; }
}