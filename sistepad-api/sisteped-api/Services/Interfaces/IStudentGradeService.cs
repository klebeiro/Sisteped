using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IStudentGradeService
    {
        Task<StudentGradeResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<StudentGradeResponseDTO>> GetAllAsync();
        Task<IEnumerable<StudentGradeResponseDTO>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentGradeResponseDTO>> GetByGradeIdAsync(int gradeId);
        Task<StudentGradeResponseDTO> CreateAsync(StudentGradeDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
