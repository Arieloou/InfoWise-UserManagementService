namespace UserManagementService.Infrastructure.RabbitMQ.Events;

public class UserPreferencesUpdatedEvent
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public List<int> CategoryIds { get; set; }
}