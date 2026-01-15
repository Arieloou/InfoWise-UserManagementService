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

        public async Task<bool> SetUserPreferences(UserPreferencesDto userPreferencesDto)
        {
            var user = await userRepository.GetUserById(userPreferencesDto.UserId);

            if (user != null)
            {
                var eventMessage = new UserPreferencesUpdatedEvent() 
                {
                    UserId = userPreferencesDto.UserId,
                    Email = user.Email,
                    ShippingHour =  userPreferencesDto.ShippingHour,
                    CategoryIds = userPreferencesDto.CategoryIds
                };
                
                await preferenceRepository.SavePreferences(userPreferencesDto);
                await producer.PublishAsync(eventMessage, Exchange, RoutingKey);
                
                return true;
            }
            
            return false;
        }

        public async Task<UserPreferencesDto?> GetUserPreferences(int userId)
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