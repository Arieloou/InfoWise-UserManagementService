using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories;

public interface IUserPreferenceRepository
{
    public Task SavePreferences(UserPreferencesDto userPreferencesDto);
    public Task<UserPreferencesDto?> GetPreferences (int userId);
}