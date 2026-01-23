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
                .Include(g => g.GridGrades)
                    .ThenInclude(gg => gg.Grade)
                        .ThenInclude(gr => gr.StudentGrades)
                .Include(g => g.GridClasses)
                    .ThenInclude(gc => gc.Class)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Grid>> GetAllAsync()
        {
            return await _context.Grids.ToListAsync();
        }

        public async Task<IEnumerable<Grid>> GetAllWithDetailsAsync()
        {
            return await _context.Grids
                .Include(g => g.GridGrades)
                    .ThenInclude(gg => gg.Grade)
                        .ThenInclude(gr => gr.StudentGrades)
                .Include(g => g.GridClasses)
                    .ThenInclude(gc => gc.Class)
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
            // Verifica se já existe
            if (await _context.GridGrades.AnyAsync(gg => gg.GridId == gridId && gg.GradeId == gradeId))
                return false;

            var gridGrade = new GridGrade
            {
                GridId = gridId,
                GradeId = gradeId,
                CreatedAt = DateTime.Now
            };

            _context.GridGrades.Add(gridGrade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGradeFromGridAsync(int gridId, int gradeId)
        {
            var gridGrade = await _context.GridGrades
                .FirstOrDefaultAsync(gg => gg.GridId == gridId && gg.GradeId == gradeId);
            
            if (gridGrade == null) return false;

            _context.GridGrades.Remove(gridGrade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
