using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Attendance>> GetByGradeIdAsync(int gradeId);
        Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Attendance>> GetByGradeAndDateAsync(int gradeId, DateTime date);
        Task<IEnumerable<Attendance>> GetByStudentAndGradeAsync(int studentId, int gradeId);
        Task<Attendance> CreateAsync(Attendance attendance);
        Task<IEnumerable<Attendance>> CreateBulkAsync(IEnumerable<Attendance> attendances);
        Task<Attendance?> UpdateAsync(Attendance attendance);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, int gradeId, DateTime date);
    }
}
