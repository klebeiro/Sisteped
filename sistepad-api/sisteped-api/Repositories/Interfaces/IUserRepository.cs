using SistepedApi.Models;
using SistepedApi.Models.Enums;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<User> CreateAsync(User user, UserCredential userCredentials);
    }
}