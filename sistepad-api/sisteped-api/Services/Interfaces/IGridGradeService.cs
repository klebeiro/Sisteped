using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IGridGradeService
    {
        Task<GridGradeResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GridGradeResponseDTO>> GetAllAsync();
        Task<IEnumerable<GridGradeResponseDTO>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<GridGradeResponseDTO>> GetByGradeIdAsync(int gradeId);
        Task<GridGradeResponseDTO> CreateAsync(GridGradeDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
