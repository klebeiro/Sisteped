using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(int id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<IEnumerable<Activity>> GetByClassIdAsync(int classId);
        Task<IEnumerable<Activity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Activity> CreateAsync(Activity activity);
        Task<Activity?> UpdateAsync(Activity activity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
