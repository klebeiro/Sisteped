using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IGridClassRepository
    {
        Task<GridClass?> GetByIdAsync(int id);
        Task<IEnumerable<GridClass>> GetAllAsync();
        Task<IEnumerable<GridClass>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<GridClass>> GetByClassIdAsync(int classId);
        Task<GridClass> CreateAsync(GridClass gridClass);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int gridId, int classId);
    }
}
