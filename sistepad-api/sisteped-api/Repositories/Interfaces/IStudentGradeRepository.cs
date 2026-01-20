using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IStudentGradeRepository
    {
        Task<StudentGrade?> GetByIdAsync(int id);
        Task<IEnumerable<StudentGrade>> GetAllAsync();
        Task<IEnumerable<StudentGrade>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentGrade>> GetByGradeIdAsync(int gradeId);
        Task<StudentGrade> CreateAsync(StudentGrade studentGrade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, int gradeId);
    }
}
