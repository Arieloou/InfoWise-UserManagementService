namespace UserManagementService.Interfaces.RabbitMQ
{
    public interface IProducer
    {
        public Task PublishAsync<T>(T message, string exchange, string routingKey);
    }
}