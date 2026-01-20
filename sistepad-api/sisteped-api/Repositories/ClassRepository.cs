using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SistepedDbContext _context;

        public ClassRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Class?> GetByCodeAsync(string code)
        {
            return await _context.Classes.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class> CreateAsync(Class classEntity)
        {
            classEntity.CreatedAt = DateTime.Now;
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Class?> UpdateAsync(Class classEntity)
        {
            var existingClass = await _context.Classes.FindAsync(classEntity.Id);
            if (existingClass == null) return null;

            existingClass.Code = classEntity.Code;
            existingClass.Name = classEntity.Name;
            existingClass.WorkloadHours = classEntity.WorkloadHours;
            existingClass.Status = classEntity.Status;
            existingClass.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingClass;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return false;

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Classes.AnyAsync(c => c.Id == id);
        }
    }
}
