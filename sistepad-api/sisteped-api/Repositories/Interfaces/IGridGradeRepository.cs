using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IGridGradeRepository
    {
        Task<GridGrade?> GetByIdAsync(int id);
        Task<IEnumerable<GridGrade>> GetAllAsync();
        Task<IEnumerable<GridGrade>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<GridGrade>> GetByGradeIdAsync(int gradeId);
        Task<GridGrade> CreateAsync(GridGrade gridGrade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int gridId, int gradeId);
    }
}
