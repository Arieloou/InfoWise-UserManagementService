using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;

namespace UserManagementService.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPreference>  UserPreferences { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserPreference>()
                .HasIndex(p => new { p.UserId, p.SubscribedCategoryId })
                .IsUnique();
        }
    }
}
