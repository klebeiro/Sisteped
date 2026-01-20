using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class GridRepository : IGridRepository
    {
        private readonly SistepedDbContext _context;

        public GridRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Grid?> GetByIdAsync(int id)
        {
            return await _context.Grids.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Grid?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Grids
                .Include(g => g.Grades)
                    .ThenInclude(gr => gr.StudentGrades)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Grid>> GetAllAsync()
        {
            return await _context.Grids.ToListAsync();
        }

        public async Task<IEnumerable<Grid>> GetAllWithDetailsAsync()
        {
            return await _context.Grids
                .Include(g => g.Grades)
                    .ThenInclude(gr => gr.StudentGrades)
                .ToListAsync();
        }

        public async Task<Grid> CreateAsync(Grid grid)
        {
            grid.CreatedAt = DateTime.Now;
            _context.Grids.Add(grid);
            await _context.SaveChangesAsync();
            return grid;
        }

        public async Task<Grid?> UpdateAsync(Grid grid)
        {
            var existingGrid = await _context.Grids.FindAsync(grid.Id);
            if (existingGrid == null) return null;

            existingGrid.Year = grid.Year;
            existingGrid.Name = grid.Name;
            existingGrid.Status = grid.Status;
            existingGrid.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingGrid;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grid = await _context.Grids.FindAsync(id);
            if (grid == null) return false;

            _context.Grids.Remove(grid);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Grids.AnyAsync(g => g.Id == id);
        }

        public async Task<bool> ExistsAsync(int year, string name)
        {
            return await _context.Grids.AnyAsync(g => g.Year == year && g.Name == name);
        }

        public async Task<bool> AddGradeToGridAsync(int gridId, int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null) return false;

            grade.GridId = gridId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGradeFromGridAsync(int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null) return false;

            grade.GridId = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
