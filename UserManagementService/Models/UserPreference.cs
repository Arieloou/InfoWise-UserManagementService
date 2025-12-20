using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementService.Models;

public class UserPreference
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public int SubscribedCategoryId { get; set; }
}