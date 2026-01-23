using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class GridGradeRepository : IGridGradeRepository
    {
        private readonly SistepedDbContext _context;

        public GridGradeRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<GridGrade?> GetByIdAsync(int id)
        {
            return await _context.GridGrades
                .Include(gg => gg.Grid)
                .Include(gg => gg.Grade)
                .FirstOrDefaultAsync(gg => gg.Id == id);
        }

        public async Task<IEnumerable<GridGrade>> GetAllAsync()
        {
            return await _context.GridGrades
                .Include(gg => gg.Grid)
                .Include(gg => gg.Grade)
                .ToListAsync();
        }

        public async Task<IEnumerable<GridGrade>> GetByGridIdAsync(int gridId)
        {
            return await _context.GridGrades
                .Include(gg => gg.Grid)
                .Include(gg => gg.Grade)
                .Where(gg => gg.GridId == gridId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GridGrade>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.GridGrades
                .Include(gg => gg.Grid)
                .Include(gg => gg.Grade)
                .Where(gg => gg.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<GridGrade> CreateAsync(GridGrade gridGrade)
        {
            gridGrade.CreatedAt = DateTime.Now;
            _context.GridGrades.Add(gridGrade);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(gridGrade.Id) ?? gridGrade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gridGrade = await _context.GridGrades.FindAsync(id);
            if (gridGrade == null) return false;

            _context.GridGrades.Remove(gridGrade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int gridId, int gradeId)
        {
            return await _context.GridGrades
                .AnyAsync(gg => gg.GridId == gridId && gg.GradeId == gradeId);
        }
    }
}
