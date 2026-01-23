using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Attendance>> GetByClassIdAsync(int classId);
        Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateTime date);
        Task<IEnumerable<Attendance>> GetByStudentAndClassAsync(int studentId, int classId);
        Task<Attendance> CreateAsync(Attendance attendance);
        Task<IEnumerable<Attendance>> CreateBulkAsync(IEnumerable<Attendance> attendances);
        Task<Attendance?> UpdateAsync(Attendance attendance);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, int classId, DateTime date);
    }
}
