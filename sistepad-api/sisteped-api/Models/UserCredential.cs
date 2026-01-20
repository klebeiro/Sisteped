using SistepedApi.Models.Enums;

namespace SistepedApi.Models
{
    public class UserCredential
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Guardian;
    }
}