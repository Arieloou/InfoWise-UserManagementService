using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using UserManagementService.Interfaces.RabbitMQ;

namespace UserManagementService.Infrastructure.RabbitMQ.Publishers
{
    public class Producer
    {
        private readonly IQueueConnection _queueConnection;
        private readonly ILogger<Producer> _logger;

        public Producer(IQueueConnection queueConnection, ILogger<Producer> logger)
        {
            _queueConnection = queueConnection;
            _logger = logger;
        }

        public async Task PublishAsync<T>(T message, string exchange, string routingKey)
        {
            try
            {
                await using var channel = await _queueConnection.CreateChannelAsync();

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var properties = new BasicProperties() { Persistent = true };

                await channel.BasicPublishAsync(
                    exchange: exchange, 
                    routingKey: routingKey, 
                    mandatory: true, 
                    basicProperties: properties, 
                    body: body
                );

                _logger.LogInformation("Message successfully published to exchange '{Exchange}'.", exchange);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to RabbitMQ.");
                throw;
            }
        }
    }   
}