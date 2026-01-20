using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<Class?> GetByIdAsync(int id);
        Task<Class?> GetByCodeAsync(string code);
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class> CreateAsync(Class classEntity);
        Task<Class?> UpdateAsync(Class classEntity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
