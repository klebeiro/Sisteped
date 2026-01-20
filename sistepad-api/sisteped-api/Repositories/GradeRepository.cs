using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly SistepedDbContext _context;

        public GradeRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await _context.Grades.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Grade>> GetAllAsync()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task<Grade> CreateAsync(Grade grade)
        {
            grade.CreatedAt = DateTime.Now;
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<Grade?> UpdateAsync(Grade grade)
        {
            var existingGrade = await _context.Grades.FindAsync(grade.Id);
            if (existingGrade == null) return null;

            existingGrade.Name = grade.Name;
            existingGrade.Level = grade.Level;
            existingGrade.Shift = grade.Shift;
            existingGrade.Status = grade.Status;
            existingGrade.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingGrade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Grades.AnyAsync(g => g.Id == id);
        }
    }
}
