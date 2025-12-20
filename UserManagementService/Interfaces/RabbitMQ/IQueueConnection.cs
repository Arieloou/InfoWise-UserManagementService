using RabbitMQ.Client;

namespace UserManagementService.Interfaces.RabbitMQ
{
    public interface IQueueConnection
    {
        Task<IChannel> CreateChannelAsync();
    }   
}