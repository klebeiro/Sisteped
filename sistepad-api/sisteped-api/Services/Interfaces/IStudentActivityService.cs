using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IStudentActivityService
    {
        Task<StudentActivityResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<StudentActivityResponseDTO>> GetAllAsync();
        Task<IEnumerable<StudentActivityResponseDTO>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentActivityResponseDTO>> GetByActivityIdAsync(int activityId);
        Task<StudentActivityResponseDTO> CreateAsync(StudentActivityCreateDTO dto);
        Task<IEnumerable<StudentActivityResponseDTO>> CreateBulkAsync(StudentActivityBulkCreateDTO dto);
        Task<StudentActivityResponseDTO?> UpdateAsync(int id, StudentActivityUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
