using Microsoft.EntityFrameworkCore;
using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure.Repositories;

public class UserPreferenceRepository(ApplicationDbContext context) : IUserPreferenceRepository
{
    public async Task SavePreferences(int userId, List<int> categoryIds)
    {
        var existingPreferences = await context.UserPreferences
            .FirstOrDefaultAsync(up => up.UserId == userId);
    
        // If the user doesn't have any preferences, they are added. Else, they are updated.
        if (existingPreferences != null)
        {
            existingPreferences.SubscribedCategoryIds = categoryIds;
            
            await context.UserPreferences.
                Where(up => up.UserId == userId).
                ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.SubscribedCategoryIds, categoryIds));
        }
        else
        {
            var newPreferences = new UserPreference
            {
                UserId = userId,
                SubscribedCategoryIds = categoryIds
            };

            await context.UserPreferences.AddAsync(newPreferences);
        }
            
        await context.SaveChangesAsync();
    }

    public async Task<UserPreferencesDto?> GetPreferences(int userId)
    {
        var userPreferences = await context.UserPreferences.
            Include(u => u.User).
            Where(u => u.UserId.Equals(userId)).
            Select(preference => new UserPreferencesDto
            {
                UserId = preference.UserId,
                CategoryIds =  preference.SubscribedCategoryIds
            } ).
            FirstOrDefaultAsync();
        
        return userPreferences;
    }
}