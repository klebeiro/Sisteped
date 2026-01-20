using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class GradeClassRepository : IGradeClassRepository
    {
        private readonly SistepedDbContext _context;

        public GradeClassRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<GradeClass?> GetByIdAsync(int id)
        {
            return await _context.GradeClasses
                .Include(gc => gc.Grade)
                .Include(gc => gc.Class)
                .FirstOrDefaultAsync(gc => gc.Id == id);
        }

        public async Task<IEnumerable<GradeClass>> GetAllAsync()
        {
            return await _context.GradeClasses
                .Include(gc => gc.Grade)
                .Include(gc => gc.Class)
                .ToListAsync();
        }

        public async Task<IEnumerable<GradeClass>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.GradeClasses
                .Include(gc => gc.Grade)
                .Include(gc => gc.Class)
                .Where(gc => gc.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GradeClass>> GetByClassIdAsync(int classId)
        {
            return await _context.GradeClasses
                .Include(gc => gc.Grade)
                .Include(gc => gc.Class)
                .Where(gc => gc.ClassId == classId)
                .ToListAsync();
        }

        public async Task<GradeClass> CreateAsync(GradeClass gradeClass)
        {
            gradeClass.CreatedAt = DateTime.Now;
            _context.GradeClasses.Add(gradeClass);
            await _context.SaveChangesAsync();
            
            // Recarregar com includes
            return await GetByIdAsync(gradeClass.Id) ?? gradeClass;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gradeClass = await _context.GradeClasses.FindAsync(id);
            if (gradeClass == null) return false;

            _context.GradeClasses.Remove(gradeClass);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int gradeId, int classId)
        {
            return await _context.GradeClasses
                .AnyAsync(gc => gc.GradeId == gradeId && gc.ClassId == classId);
        }
    }
}
