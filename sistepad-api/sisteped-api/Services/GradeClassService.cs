using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class GradeClassService : IGradeClassService
    {
        private readonly IGradeClassRepository _gradeClassRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GradeClassService(
            IGradeClassRepository gradeClassRepository,
            IGradeRepository gradeRepository,
            IClassRepository classRepository,
            IMapper mapper)
        {
            _gradeClassRepository = gradeClassRepository;
            _gradeRepository = gradeRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<GradeClassResponseDTO?> GetByIdAsync(int id)
        {
            var gradeClass = await _gradeClassRepository.GetByIdAsync(id);
            return gradeClass != null ? _mapper.Map<GradeClassResponseDTO>(gradeClass) : null;
        }

        public async Task<IEnumerable<GradeClassResponseDTO>> GetAllAsync()
        {
            var gradeClasses = await _gradeClassRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GradeClassResponseDTO>>(gradeClasses);
        }

        public async Task<IEnumerable<GradeClassResponseDTO>> GetByGradeIdAsync(int gradeId)
        {
            var gradeClasses = await _gradeClassRepository.GetByGradeIdAsync(gradeId);
            return _mapper.Map<IEnumerable<GradeClassResponseDTO>>(gradeClasses);
        }

        public async Task<IEnumerable<GradeClassResponseDTO>> GetByClassIdAsync(int classId)
        {
            var gradeClasses = await _gradeClassRepository.GetByClassIdAsync(classId);
            return _mapper.Map<IEnumerable<GradeClassResponseDTO>>(gradeClasses);
        }

        public async Task<GradeClassResponseDTO> CreateAsync(GradeClassDTO dto)
        {
            if (!await _gradeRepository.ExistsAsync(dto.GradeId))
            {
                throw new Exception("Série não encontrada.");
            }

            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            if (await _gradeClassRepository.ExistsAsync(dto.GradeId, dto.ClassId))
            {
                throw new Exception("Esta matéria já está associada a esta série.");
            }

            var gradeClass = new GradeClass
            {
                GradeId = dto.GradeId,
                ClassId = dto.ClassId
            };

            var createdGradeClass = await _gradeClassRepository.CreateAsync(gradeClass);
            return _mapper.Map<GradeClassResponseDTO>(createdGradeClass);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gradeClassRepository.DeleteAsync(id);
        }
    }
}
