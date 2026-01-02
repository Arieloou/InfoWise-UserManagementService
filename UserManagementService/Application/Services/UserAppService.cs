using UserManagementService.Infrastructure.DTOs;
using UserManagementService.Infrastructure.RabbitMQ.Events;
using UserManagementService.Interfaces.RabbitMQ;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Models;

namespace UserManagementService.Application.Services
{
    public class UserAppService(
        IUserRepository userRepository, 
        IUserPreferenceRepository preferenceRepository, 
        IProducer producer)
    {
        private const string Exchange = "user.exchange";
        private const string RoutingKey = "preferences.configured";

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
    
                await producer.PublishAsync(eventMessage, Exchange, RoutingKey);
            }
        }

        public async Task<UserPreference?> GetUserPreferences(int userId)
        {
            return await preferenceRepository.GetPreferences(userId);
        }

        public async Task<JwtResponse> LoginUser(UserDto userDto)
        {
            return await userRepository.Login(userDto);
        }
        
        public async Task<JwtResponse> RegisterUser(UserDto userDto)
        {
            return await userRepository.Register(userDto);
        }
    }   
}