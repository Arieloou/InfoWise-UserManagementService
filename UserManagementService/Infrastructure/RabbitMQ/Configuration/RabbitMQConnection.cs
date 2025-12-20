using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using UserManagementService.Interfaces.RabbitMQ;

namespace UserManagementService.Infrastructure.RabbitMQ.Configuration
{
    public class RabbitMQConnection : IQueueConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private static readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1); 
        private readonly ILogger<RabbitMQConnection> _logger;

        private IConnection? _connection;

        public RabbitMQConnection(IOptions<RabbitMQConfiguration> options, ILogger<RabbitMQConnection> logger)
        {
            _logger = logger;
            var config = options.Value;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                VirtualHost = config.VirtualHost,
                UserName = config.Username,
                Password = config.Password 
            };
        }

        public bool IsConnected => _connection != null && _connection.IsOpen;

        private async Task TryConnectAsync(CancellationToken cancellationToken = default)
        {
            if (IsConnected) return;

            _logger.LogInformation("Intentando establecer conexión con RabbitMQ...");

            await _connectionLock.WaitAsync(cancellationToken);
            try
            {
                if (IsConnected) return;

                _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                _logger.LogInformation("Conexión a RabbitMQ establecida exitosamente.");
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogCritical(ex, "No se pudo conectar a RabbitMQ. El host es inalcanzable.");
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            if (!IsConnected)
            {
                await TryConnectAsync();
            }

            return await _connection!.CreateChannelAsync();
        }

        public void Dispose()
        {
            try
            {
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error al cerrar la conexión de RabbitMQ.");
            }
        }
    }   
}