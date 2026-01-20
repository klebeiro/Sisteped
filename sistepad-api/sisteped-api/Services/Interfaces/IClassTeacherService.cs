using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IClassTeacherService
    {
        Task<ClassTeacherResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ClassTeacherResponseDTO>> GetAllAsync();
        Task<IEnumerable<ClassTeacherResponseDTO>> GetByClassIdAsync(int classId);
        Task<IEnumerable<ClassTeacherResponseDTO>> GetByTeacherIdAsync(int teacherId);
        Task<ClassTeacherResponseDTO> CreateAsync(ClassTeacherDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
