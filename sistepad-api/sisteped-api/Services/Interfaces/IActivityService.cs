using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ActivityResponseDTO>> GetAllAsync();
        Task<IEnumerable<ActivityResponseDTO>> GetByClassIdAsync(int classId);
        Task<IEnumerable<ActivityResponseDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ActivityResponseDTO> CreateAsync(ActivityCreateDTO dto);
        Task<ActivityResponseDTO?> UpdateAsync(int id, ActivityUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
