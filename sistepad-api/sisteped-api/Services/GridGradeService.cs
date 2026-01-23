using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class GridGradeService : IGridGradeService
    {
        private readonly IGridGradeRepository _gridGradeRepository;
        private readonly IGridRepository _gridRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;

        public GridGradeService(
            IGridGradeRepository gridGradeRepository,
            IGridRepository gridRepository,
            IGradeRepository gradeRepository,
            IMapper mapper)
        {
            _gridGradeRepository = gridGradeRepository;
            _gridRepository = gridRepository;
            _gradeRepository = gradeRepository;
            _mapper = mapper;
        }

        public async Task<GridGradeResponseDTO?> GetByIdAsync(int id)
        {
            var gridGrade = await _gridGradeRepository.GetByIdAsync(id);
            return gridGrade != null ? _mapper.Map<GridGradeResponseDTO>(gridGrade) : null;
        }

        public async Task<IEnumerable<GridGradeResponseDTO>> GetAllAsync()
        {
            var gridGrades = await _gridGradeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GridGradeResponseDTO>>(gridGrades);
        }

        public async Task<IEnumerable<GridGradeResponseDTO>> GetByGridIdAsync(int gridId)
        {
            var gridGrades = await _gridGradeRepository.GetByGridIdAsync(gridId);
            return _mapper.Map<IEnumerable<GridGradeResponseDTO>>(gridGrades);
        }

        public async Task<IEnumerable<GridGradeResponseDTO>> GetByGradeIdAsync(int gradeId)
        {
            var gridGrades = await _gridGradeRepository.GetByGradeIdAsync(gradeId);
            return _mapper.Map<IEnumerable<GridGradeResponseDTO>>(gridGrades);
        }

        public async Task<GridGradeResponseDTO> CreateAsync(GridGradeDTO dto)
        {
            if (!await _gridRepository.ExistsAsync(dto.GridId))
            {
                throw new Exception("Grade curricular não encontrada.");
            }

            if (!await _gradeRepository.ExistsAsync(dto.GradeId))
            {
                throw new Exception("Turma não encontrada.");
            }

            if (await _gridGradeRepository.ExistsAsync(dto.GridId, dto.GradeId))
            {
                throw new Exception("Esta turma já está vinculada a esta grade curricular.");
            }

            var gridGrade = new GridGrade
            {
                GridId = dto.GridId,
                GradeId = dto.GradeId
            };

            var createdGridGrade = await _gridGradeRepository.CreateAsync(gridGrade);
            return _mapper.Map<GridGradeResponseDTO>(createdGridGrade);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gridGradeRepository.DeleteAsync(id);
        }
    }
}
