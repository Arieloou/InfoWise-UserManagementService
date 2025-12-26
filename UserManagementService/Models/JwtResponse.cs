namespace UserManagementService.Models
{

    public class JwtResponse
    {
        public required string Token { get; set; }
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }   
    }
}