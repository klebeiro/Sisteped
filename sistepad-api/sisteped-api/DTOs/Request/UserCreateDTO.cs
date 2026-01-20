using SistepedApi.Models.Enums;

namespace SistepedApi.DTOs.Request
{
    public class UserCreateDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Guardian;
    }
}