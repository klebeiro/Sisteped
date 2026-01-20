using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly SistepedDbContext _context;

        public ActivityRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Grade)
                    .ThenInclude(g => g.Grid)
                .Include(a => a.StudentActivities)
                    .ThenInclude(sa => sa.Student)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Grade)
                .Include(a => a.StudentActivities)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.Activities
                .Include(a => a.Grade)
                .Include(a => a.StudentActivities)
                .Where(a => a.GradeId == gradeId)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Activities
                .Include(a => a.Grade)
                .Include(a => a.StudentActivities)
                .Where(a => a.ApplicationDate >= startDate && a.ApplicationDate <= endDate)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            activity.CreatedAt = DateTime.Now;
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(activity.Id) ?? activity;
        }

        public async Task<Activity?> UpdateAsync(Activity activity)
        {
            var existingActivity = await _context.Activities.FindAsync(activity.Id);
            if (existingActivity == null) return null;

            existingActivity.Title = activity.Title;
            existingActivity.Description = activity.Description;
            existingActivity.ApplicationDate = activity.ApplicationDate;
            existingActivity.MaxScore = activity.MaxScore;
            existingActivity.Status = activity.Status;
            existingActivity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(existingActivity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Activities.AnyAsync(a => a.Id == id);
        }
    }
}
