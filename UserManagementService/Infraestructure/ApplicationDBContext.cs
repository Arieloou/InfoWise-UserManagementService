using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;

namespace UserManagementService.Infraestructure
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
