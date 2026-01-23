using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IGridClassService
    {
        Task<GridClassResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GridClassResponseDTO>> GetAllAsync();
        Task<IEnumerable<GridClassResponseDTO>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<GridClassResponseDTO>> GetByClassIdAsync(int classId);
        Task<GridClassResponseDTO> CreateAsync(GridClassDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
