using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IClassTeacherRepository
    {
        Task<ClassTeacher?> GetByIdAsync(int id);
        Task<IEnumerable<ClassTeacher>> GetAllAsync();
        Task<IEnumerable<ClassTeacher>> GetByClassIdAsync(int classId);
        Task<IEnumerable<ClassTeacher>> GetByTeacherIdAsync(int teacherId);
        Task<ClassTeacher> CreateAsync(ClassTeacher classTeacher);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int classId, int teacherId);
    }
}
