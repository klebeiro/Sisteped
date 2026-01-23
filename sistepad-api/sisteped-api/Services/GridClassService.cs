using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class GridClassService : IGridClassService
    {
        private readonly IGridClassRepository _gridClassRepository;
        private readonly IGridRepository _gridRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GridClassService(
            IGridClassRepository gridClassRepository,
            IGridRepository gridRepository,
            IClassRepository classRepository,
            IMapper mapper)
        {
            _gridClassRepository = gridClassRepository;
            _gridRepository = gridRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<GridClassResponseDTO?> GetByIdAsync(int id)
        {
            var gridClass = await _gridClassRepository.GetByIdAsync(id);
            return gridClass != null ? _mapper.Map<GridClassResponseDTO>(gridClass) : null;
        }

        public async Task<IEnumerable<GridClassResponseDTO>> GetAllAsync()
        {
            var gridClasses = await _gridClassRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GridClassResponseDTO>>(gridClasses);
        }

        public async Task<IEnumerable<GridClassResponseDTO>> GetByGridIdAsync(int gridId)
        {
            var gridClasses = await _gridClassRepository.GetByGridIdAsync(gridId);
            return _mapper.Map<IEnumerable<GridClassResponseDTO>>(gridClasses);
        }

        public async Task<IEnumerable<GridClassResponseDTO>> GetByClassIdAsync(int classId)
        {
            var gridClasses = await _gridClassRepository.GetByClassIdAsync(classId);
            return _mapper.Map<IEnumerable<GridClassResponseDTO>>(gridClasses);
        }

        public async Task<GridClassResponseDTO> CreateAsync(GridClassDTO dto)
        {
            if (!await _gridRepository.ExistsAsync(dto.GridId))
            {
                throw new Exception("Grade curricular não encontrada.");
            }

            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            if (await _gridClassRepository.ExistsAsync(dto.GridId, dto.ClassId))
            {
                throw new Exception("Esta matéria já está associada a esta grade curricular.");
            }

            var gridClass = new GridClass
            {
                GridId = dto.GridId,
                ClassId = dto.ClassId
            };

            var createdGridClass = await _gridClassRepository.CreateAsync(gridClass);
            return _mapper.Map<GridClassResponseDTO>(createdGridClass);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gridClassRepository.DeleteAsync(id);
        }
    }
}
