using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceResponseDTO>> GetAllAsync();
        Task<IEnumerable<AttendanceResponseDTO>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<AttendanceResponseDTO>> GetByGradeIdAsync(int gradeId);
        Task<IEnumerable<AttendanceResponseDTO>> GetByDateAsync(DateTime date);
        Task<IEnumerable<AttendanceResponseDTO>> GetByGradeAndDateAsync(int gradeId, DateTime date);
        Task<IEnumerable<AttendanceResponseDTO>> GetByStudentAndGradeAsync(int studentId, int gradeId);
        Task<AttendanceResponseDTO> CreateAsync(AttendanceCreateDTO dto);
        Task<IEnumerable<AttendanceResponseDTO>> CreateBulkAsync(AttendanceBulkCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
