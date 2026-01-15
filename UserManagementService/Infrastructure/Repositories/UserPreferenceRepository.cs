using Microsoft.EntityFrameworkCore;
using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure.Repositories;

public class UserPreferenceRepository(ApplicationDbContext context) : IUserPreferenceRepository
{
    public async Task SavePreferences(UserPreferencesDto userPreferencesDto)
    {
        var existingPreferences = await context.UserPreferences
            .FirstOrDefaultAsync(up => up.UserId == userPreferencesDto.UserId);
    
        // If the user doesn't have any preferences, they are added. Else, they are updated.
        if (existingPreferences != null)
        {
            existingPreferences.SubscribedCategoryIds = userPreferencesDto.CategoryIds;
            
            await context.UserPreferences.
                Where(up => up.UserId == userPreferencesDto.UserId).
                ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.SubscribedCategoryIds, userPreferencesDto.CategoryIds)
                    .SetProperty(p => p.ShippingHour, userPreferencesDto.ShippingHour));
        }
        else
        {
            var newPreferences = new UserPreference
            {
                UserId = userPreferencesDto.UserId,
                ShippingHour = userPreferencesDto.ShippingHour,
                SubscribedCategoryIds = userPreferencesDto.CategoryIds
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
                ShippingHour =  preference.ShippingHour,
                CategoryIds =  preference.SubscribedCategoryIds
            } ).
            FirstOrDefaultAsync();
        
        return userPreferences;
    }
}