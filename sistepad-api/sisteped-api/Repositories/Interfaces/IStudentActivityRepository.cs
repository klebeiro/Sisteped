using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IStudentActivityRepository
    {
        Task<StudentActivity?> GetByIdAsync(int id);
        Task<IEnumerable<StudentActivity>> GetAllAsync();
        Task<IEnumerable<StudentActivity>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentActivity>> GetByActivityIdAsync(int activityId);
        Task<StudentActivity?> GetByStudentAndActivityAsync(int studentId, int activityId);
        Task<StudentActivity> CreateAsync(StudentActivity studentActivity);
        Task<IEnumerable<StudentActivity>> CreateBulkAsync(IEnumerable<StudentActivity> studentActivities);
        Task<StudentActivity?> UpdateAsync(StudentActivity studentActivity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, int activityId);
    }
}
