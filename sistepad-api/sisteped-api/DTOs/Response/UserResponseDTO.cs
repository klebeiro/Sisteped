using SistepedApi.Models.Enums;

namespace SistepedApi.DTOs.Response
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RoleName => Role.ToString();
        public bool Status { get; set; }
    }
}