using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IClassService
    {
        Task<ClassResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ClassResponseDTO>> GetAllAsync();
        Task<ClassResponseDTO> CreateAsync(ClassCreateDTO dto);
        Task<ClassResponseDTO?> UpdateAsync(int id, ClassUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
