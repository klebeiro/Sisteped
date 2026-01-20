using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IGradeService
    {
        Task<GradeResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GradeResponseDTO>> GetAllAsync();
        Task<GradeResponseDTO> CreateAsync(GradeCreateDTO dto);
        Task<GradeResponseDTO?> UpdateAsync(int id, GradeUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
