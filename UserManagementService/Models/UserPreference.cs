using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementService.Models;

public class UserPreference
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User? User { get; set; }
    public int SubscribedCategoryId { get; set; }
}