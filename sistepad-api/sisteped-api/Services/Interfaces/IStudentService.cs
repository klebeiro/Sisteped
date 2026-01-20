using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<StudentResponseDTO>> GetAllAsync();
        Task<IEnumerable<StudentResponseDTO>> GetByGuardianIdAsync(int guardianId);
        Task<StudentResponseDTO> CreateAsync(StudentCreateDTO dto);
        Task<StudentResponseDTO?> UpdateAsync(int id, StudentUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
