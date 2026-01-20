using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class GridService : IGridService
    {
        private readonly IGridRepository _gridRepository;
        private readonly IGradeRepository _gradeRepository;

        public GridService(IGridRepository gridRepository, IGradeRepository gradeRepository)
        {
            _gridRepository = gridRepository;
            _gradeRepository = gradeRepository;
        }

        public async Task<GridResponseDTO?> GetByIdAsync(int id)
        {
            var grid = await _gridRepository.GetByIdWithDetailsAsync(id);
            return grid != null ? MapToResponse(grid) : null;
        }

        public async Task<IEnumerable<GridResponseDTO>> GetAllAsync()
        {
            var grids = await _gridRepository.GetAllWithDetailsAsync();
            return grids.Select(MapToResponse);
        }

        public async Task<GridResponseDTO> CreateAsync(GridCreateDTO dto)
        {
            if (await _gridRepository.ExistsAsync(dto.Year, dto.Name))
            {
                throw new Exception("Já existe uma grade com este ano e nome.");
            }

            var grid = new Grid
            {
                Year = dto.Year,
                Name = dto.Name,
                Status = dto.Status
            };

            var createdGrid = await _gridRepository.CreateAsync(grid);
            return MapToResponse(createdGrid);
        }

        public async Task<GridResponseDTO?> UpdateAsync(int id, GridUpdateDTO dto)
        {
            var existingGrid = await _gridRepository.GetByIdAsync(id);
            if (existingGrid == null) return null;

            var gridWithSameYearName = await _gridRepository.ExistsAsync(dto.Year, dto.Name);
            if (gridWithSameYearName && (existingGrid.Year != dto.Year || existingGrid.Name != dto.Name))
            {
                throw new Exception("Já existe outra grade com este ano e nome.");
            }

            existingGrid.Year = dto.Year;
            existingGrid.Name = dto.Name;
            existingGrid.Status = dto.Status;

            var updatedGrid = await _gridRepository.UpdateAsync(existingGrid);
            if (updatedGrid == null) return null;

            var gridWithDetails = await _gridRepository.GetByIdWithDetailsAsync(id);
            return gridWithDetails != null ? MapToResponse(gridWithDetails) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gridRepository.DeleteAsync(id);
        }

        public async Task<bool> AddGradeToGridAsync(GridGradeDTO dto)
        {
            if (!await _gridRepository.ExistsAsync(dto.GridId))
            {
                throw new Exception("Grade não encontrada.");
            }

            if (!await _gradeRepository.ExistsAsync(dto.GradeId))
            {
                throw new Exception("Série não encontrada.");
            }

            return await _gridRepository.AddGradeToGridAsync(dto.GridId, dto.GradeId);
        }

        public async Task<bool> RemoveGradeFromGridAsync(int gradeId)
        {
            if (!await _gradeRepository.ExistsAsync(gradeId))
            {
                throw new Exception("Série não encontrada.");
            }

            return await _gridRepository.RemoveGradeFromGridAsync(gradeId);
        }

        private GridResponseDTO MapToResponse(Grid grid)
        {
            var grades = grid.Grades?.ToList() ?? new List<Grade>();
            
            var totalStudents = grades
                .SelectMany(g => g.StudentGrades ?? new List<StudentGrade>())
                .Select(sg => sg.StudentId)
                .Distinct()
                .Count();

            return new GridResponseDTO
            {
                Id = grid.Id,
                Year = grid.Year,
                Name = grid.Name,
                Status = grid.Status,
                CreatedAt = grid.CreatedAt,
                UpdatedAt = grid.UpdatedAt,
                Grades = grades.Select(g => g.Id).ToList(),
                TotalClasses = grades.Count,
                TotalStudents = totalStudents
            };
        }
    }
}
