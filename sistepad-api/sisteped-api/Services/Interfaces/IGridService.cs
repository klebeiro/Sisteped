using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IGridService
    {
        Task<GridResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GridResponseDTO>> GetAllAsync();
        Task<GridResponseDTO> CreateAsync(GridCreateDTO dto);
        Task<GridResponseDTO?> UpdateAsync(int id, GridUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddGradeToGridAsync(GridGradeDTO dto);
        Task<bool> RemoveGradeFromGridAsync(int gradeId);
    }
}
