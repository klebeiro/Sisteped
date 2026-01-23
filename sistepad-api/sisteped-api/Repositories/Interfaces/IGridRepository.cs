using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IGridRepository
    {
        Task<Grid?> GetByIdAsync(int id);
        Task<Grid?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Grid>> GetAllAsync();
        Task<IEnumerable<Grid>> GetAllWithDetailsAsync();
        Task<Grid> CreateAsync(Grid grid);
        Task<Grid?> UpdateAsync(Grid grid);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(int year, string name);
        Task<bool> AddGradeToGridAsync(int gridId, int gradeId);
        Task<bool> RemoveGradeFromGridAsync(int gridId, int gradeId);
    }
}
