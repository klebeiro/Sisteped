using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class GridClassRepository : IGridClassRepository
    {
        private readonly SistepedDbContext _context;

        public GridClassRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<GridClass?> GetByIdAsync(int id)
        {
            return await _context.GridClasses
                .Include(gc => gc.Grid)
                .Include(gc => gc.Class)
                .FirstOrDefaultAsync(gc => gc.Id == id);
        }

        public async Task<IEnumerable<GridClass>> GetAllAsync()
        {
            return await _context.GridClasses
                .Include(gc => gc.Grid)
                .Include(gc => gc.Class)
                .ToListAsync();
        }

        public async Task<IEnumerable<GridClass>> GetByGridIdAsync(int gridId)
        {
            return await _context.GridClasses
                .Include(gc => gc.Grid)
                .Include(gc => gc.Class)
                .Where(gc => gc.GridId == gridId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GridClass>> GetByClassIdAsync(int classId)
        {
            return await _context.GridClasses
                .Include(gc => gc.Grid)
                .Include(gc => gc.Class)
                .Where(gc => gc.ClassId == classId)
                .ToListAsync();
        }

        public async Task<GridClass> CreateAsync(GridClass gridClass)
        {
            gridClass.CreatedAt = DateTime.Now;
            _context.GridClasses.Add(gridClass);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(gridClass.Id) ?? gridClass;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gridClass = await _context.GridClasses.FindAsync(id);
            if (gridClass == null) return false;

            _context.GridClasses.Remove(gridClass);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int gridId, int classId)
        {
            return await _context.GridClasses
                .AnyAsync(gc => gc.GridId == gridId && gc.ClassId == classId);
        }
    }
}
