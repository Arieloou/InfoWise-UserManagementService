using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Models;

namespace UserManagementService.Interfaces.Repositories;

public interface IUserPreferenceRepository
{
    public Task SavePreferences(int userId, List<int> categoryIds);
    public Task<UserPreferencesDto?> GetPreferences (int userId);
}