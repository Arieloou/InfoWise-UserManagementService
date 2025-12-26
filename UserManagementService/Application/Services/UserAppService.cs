using UserManagementService.Infrastructure.RabbitMQ.Events;
using UserManagementService.Interfaces.RabbitMQ;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Models;

namespace UserManagementService.Application.Services
{
    public class UserAppService(IUserRepository userRepository, IUserPreferenceRepository preferenceRepository, IProducer producer)
    {
        public async Task SetUserPreferences(int userId, int[] categoryIds)
        {
            // 1. Guardar en UserPreferences
            await preferenceRepository.SavePreferences(userId, categoryIds);
    
            // 2. Obtener email del usuario
            var user = await userRepository.GetUserById(userId);

            if (user != null)
            {
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

        public async Task<UserPreference?> GetUserPreferences(int userId)
        {
            return await preferenceRepository.GetPreferences(userId);
        }

        public async Task<JwtResponse> LoginUser(User user)
        {
            return await userRepository.Login(user);
        }
        
        public async Task<JwtResponse> RegisterUser(User user)
        {
            return await userRepository.Register(user);
        }
    }   
}