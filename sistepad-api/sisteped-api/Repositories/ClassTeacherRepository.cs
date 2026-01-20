using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class ClassTeacherRepository : IClassTeacherRepository
    {
        private readonly SistepedDbContext _context;

        public ClassTeacherRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<ClassTeacher?> GetByIdAsync(int id)
        {
            return await _context.ClassTeachers
                .Include(ct => ct.Class)
                .Include(ct => ct.Teacher)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<IEnumerable<ClassTeacher>> GetAllAsync()
        {
            return await _context.ClassTeachers
                .Include(ct => ct.Class)
                .Include(ct => ct.Teacher)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClassTeacher>> GetByClassIdAsync(int classId)
        {
            return await _context.ClassTeachers
                .Include(ct => ct.Class)
                .Include(ct => ct.Teacher)
                .Where(ct => ct.ClassId == classId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClassTeacher>> GetByTeacherIdAsync(int teacherId)
        {
            return await _context.ClassTeachers
                .Include(ct => ct.Class)
                .Include(ct => ct.Teacher)
                .Where(ct => ct.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<ClassTeacher> CreateAsync(ClassTeacher classTeacher)
        {
            classTeacher.CreatedAt = DateTime.Now;
            _context.ClassTeachers.Add(classTeacher);
            await _context.SaveChangesAsync();
            
            // Recarregar com includes
            return await GetByIdAsync(classTeacher.Id) ?? classTeacher;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classTeacher = await _context.ClassTeachers.FindAsync(id);
            if (classTeacher == null) return false;

            _context.ClassTeachers.Remove(classTeacher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int classId, int teacherId)
        {
            return await _context.ClassTeachers
                .AnyAsync(ct => ct.ClassId == classId && ct.TeacherId == teacherId);
        }
    }
}
