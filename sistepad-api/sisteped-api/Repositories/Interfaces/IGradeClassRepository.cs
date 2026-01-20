using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IGradeClassRepository
    {
        Task<GradeClass?> GetByIdAsync(int id);
        Task<IEnumerable<GradeClass>> GetAllAsync();
        Task<IEnumerable<GradeClass>> GetByGradeIdAsync(int gradeId);
        Task<IEnumerable<GradeClass>> GetByClassIdAsync(int classId);
        Task<GradeClass> CreateAsync(GradeClass gradeClass);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int gradeId, int classId);
    }
}
