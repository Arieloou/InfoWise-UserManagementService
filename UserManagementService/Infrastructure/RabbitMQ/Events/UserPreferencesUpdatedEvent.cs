namespace UserManagementService.Infrastructure.RabbitMQ.Events;

public class UserPreferencesUpdatedEvent
{
    public required int UserId { get; set; }
    public required string Email { get; set; }
    public List<int>? CategoryIds { get; set; }
}