using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;

        public GradeService(IGradeRepository gradeRepository, IMapper mapper)
        {
            _gradeRepository = gradeRepository;
            _mapper = mapper;
        }

        public async Task<GradeResponseDTO?> GetByIdAsync(int id)
        {
            var grade = await _gradeRepository.GetByIdAsync(id);
            return grade != null ? _mapper.Map<GradeResponseDTO>(grade) : null;
        }

        public async Task<IEnumerable<GradeResponseDTO>> GetAllAsync()
        {
            var grades = await _gradeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GradeResponseDTO>>(grades);
        }

        public async Task<GradeResponseDTO> CreateAsync(GradeCreateDTO dto)
        {
            var grade = _mapper.Map<Grade>(dto);
            var createdGrade = await _gradeRepository.CreateAsync(grade);
            return _mapper.Map<GradeResponseDTO>(createdGrade);
        }

        public async Task<GradeResponseDTO?> UpdateAsync(int id, GradeUpdateDTO dto)
        {
            var existingGrade = await _gradeRepository.GetByIdAsync(id);
            if (existingGrade == null) return null;

            existingGrade.Name = dto.Name;
            existingGrade.Level = dto.Level;
            existingGrade.Shift = dto.Shift;
            existingGrade.Status = dto.Status;

            var updatedGrade = await _gradeRepository.UpdateAsync(existingGrade);
            return updatedGrade != null ? _mapper.Map<GradeResponseDTO>(updatedGrade) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gradeRepository.DeleteAsync(id);
        }
    }
}
