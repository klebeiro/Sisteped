using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade?> GetByIdAsync(int id);
        Task<IEnumerable<Grade>> GetAllAsync();
        Task<Grade> CreateAsync(Grade grade);
        Task<Grade?> UpdateAsync(Grade grade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
