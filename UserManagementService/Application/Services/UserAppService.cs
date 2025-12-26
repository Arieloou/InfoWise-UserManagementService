using UserManagementService.Infrastructure.RabbitMQ.Events;
using UserManagementService.Interfaces.RabbitMQ;
using UserManagementService.Interfaces.Repositories;

namespace UserManagementService.Application.Services
{
    public class UserAppService(IUserRepository userRepository, IProducer producer)
    {
        public async Task SetUserPreferences(int userId, int[] categoryIds)
        {
            // 1. Guardar en UserDB (tabla UserPreferences)
            await userRepository.SavePreferences(userId, categoryIds);
    
            // 2. Obtener email del usuario
            var user = await userRepository.GetUserById(userId);

            // 3. Publicar evento de integración para que otros servicios se enteren
            var eventMessage = new UserPreferencesUpdatedEvent() 
            {
                UserId = userId,
                Email = user.Email,
                CategoryIds = categoryIds.ToList()
            };
    
            // Usando MassTransit o tu cliente RabbitMQ
            await producer.PublishAsync(eventMessage, "user.exchange", "preferences.configured");
        }
    }   
}