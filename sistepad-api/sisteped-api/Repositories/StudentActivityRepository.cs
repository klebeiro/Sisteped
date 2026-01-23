using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class StudentActivityRepository : IStudentActivityRepository
    {
        private readonly SistepedDbContext _context;

        public StudentActivityRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<StudentActivity?> GetByIdAsync(int id)
        {
            return await _context.StudentActivities
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.Guardian)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Class)
                .FirstOrDefaultAsync(sa => sa.Id == id);
        }

        public async Task<IEnumerable<StudentActivity>> GetAllAsync()
        {
            return await _context.StudentActivities
                .Include(sa => sa.Student)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Class)
                .OrderByDescending(sa => sa.Activity.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentActivity>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentActivities
                .Include(sa => sa.Student)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Class)
                .Where(sa => sa.StudentId == studentId)
                .OrderByDescending(sa => sa.Activity.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentActivity>> GetByActivityIdAsync(int activityId)
        {
            return await _context.StudentActivities
                .Include(sa => sa.Student)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Class)
                .Where(sa => sa.ActivityId == activityId)
                .OrderBy(sa => sa.Student.Name)
                .ToListAsync();
        }

        public async Task<StudentActivity?> GetByStudentAndActivityAsync(int studentId, int activityId)
        {
            return await _context.StudentActivities
                .Include(sa => sa.Student)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Class)
                .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.ActivityId == activityId);
        }

        public async Task<StudentActivity> CreateAsync(StudentActivity studentActivity)
        {
            studentActivity.CreatedAt = DateTime.Now;
            _context.StudentActivities.Add(studentActivity);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(studentActivity.Id) ?? studentActivity;
        }

        public async Task<IEnumerable<StudentActivity>> CreateBulkAsync(IEnumerable<StudentActivity> studentActivities)
        {
            var activityList = studentActivities.ToList();
            foreach (var sa in activityList)
            {
                sa.CreatedAt = DateTime.Now;
            }
            
            _context.StudentActivities.AddRange(activityList);
            await _context.SaveChangesAsync();
            
            return activityList;
        }

        public async Task<StudentActivity?> UpdateAsync(StudentActivity studentActivity)
        {
            var existingSA = await _context.StudentActivities.FindAsync(studentActivity.Id);
            if (existingSA == null) return null;

            existingSA.Score = studentActivity.Score;
            existingSA.Remarks = studentActivity.Remarks;
            existingSA.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(existingSA.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var studentActivity = await _context.StudentActivities.FindAsync(id);
            if (studentActivity == null) return false;

            _context.StudentActivities.Remove(studentActivity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int studentId, int activityId)
        {
            return await _context.StudentActivities
                .AnyAsync(sa => sa.StudentId == studentId && sa.ActivityId == activityId);
        }
    }
}
