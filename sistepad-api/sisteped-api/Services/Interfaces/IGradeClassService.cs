using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IGradeClassService
    {
        Task<GradeClassResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GradeClassResponseDTO>> GetAllAsync();
        Task<IEnumerable<GradeClassResponseDTO>> GetByGradeIdAsync(int gradeId);
        Task<IEnumerable<GradeClassResponseDTO>> GetByClassIdAsync(int classId);
        Task<GradeClassResponseDTO> CreateAsync(GradeClassDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
