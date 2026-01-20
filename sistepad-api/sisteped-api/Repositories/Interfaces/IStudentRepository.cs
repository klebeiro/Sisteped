using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByEnrollmentAsync(string enrollment);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<IEnumerable<Student>> GetByGuardianIdAsync(int guardianId);
        Task<Student> CreateAsync(Student student);
        Task<Student?> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
