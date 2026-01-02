using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Infrastructure.DTOs;

public class UserDto
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}