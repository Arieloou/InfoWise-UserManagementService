using Microsoft.EntityFrameworkCore;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure.Repositories;

public class UserPreferenceRepository(ApplicationDbContext context) : IUserPreferenceRepository
{
    public async Task SavePreferences(int userId, int[] categoryIds)
    {
        var existingPreferences = await context.UserPreferences
            .Where(up => up.UserId == userId)
            .ToListAsync();
            
        // We delete the existing user preference to reset all of them
        if (existingPreferences.Any())
        {
            context.UserPreferences.RemoveRange(existingPreferences);
        }
            
        var newPreferences = categoryIds.Select(categoryId => new UserPreference
        {
            UserId = userId,
            SubscribedCategoryId = categoryId
        }).ToList();

        await context.UserPreferences.AddRangeAsync(newPreferences);
            
        await context.SaveChangesAsync();
    }

    public async Task<UserPreference?> GetPreferences(int userId)
    {
        return await context.UserPreferences.
            Include(u => u.User).
            Where(u => u.UserId.Equals(userId)).
            FirstOrDefaultAsync();
    }
}