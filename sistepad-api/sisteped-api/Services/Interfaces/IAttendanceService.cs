using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceResponseDTO>> GetAllAsync();
        Task<IEnumerable<AttendanceResponseDTO>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<AttendanceResponseDTO>> GetByClassIdAsync(int classId);
        Task<IEnumerable<AttendanceResponseDTO>> GetByDateAsync(DateTime date);
        Task<IEnumerable<AttendanceResponseDTO>> GetByClassAndDateAsync(int classId, DateTime date);
        Task<IEnumerable<AttendanceResponseDTO>> GetByStudentAndClassAsync(int studentId, int classId);
        Task<AttendanceResponseDTO> CreateAsync(AttendanceCreateDTO dto);
        Task<IEnumerable<AttendanceResponseDTO>> CreateBulkAsync(AttendanceBulkCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
