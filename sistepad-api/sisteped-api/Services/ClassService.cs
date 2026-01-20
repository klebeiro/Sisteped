using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Resources;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ClassResponseDTO?> GetByIdAsync(int id)
        {
            var classEntity = await _classRepository.GetByIdAsync(id);
            return classEntity != null ? _mapper.Map<ClassResponseDTO>(classEntity) : null;
        }

        public async Task<IEnumerable<ClassResponseDTO>> GetAllAsync()
        {
            var classes = await _classRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClassResponseDTO>>(classes);
        }

        public async Task<ClassResponseDTO> CreateAsync(ClassCreateDTO dto)
        {
            var existingClass = await _classRepository.GetByCodeAsync(dto.Code);
            if (existingClass != null)
            {
                throw new Exception("Já existe uma matéria com este código.");
            }

            var classEntity = _mapper.Map<Class>(dto);
            var createdClass = await _classRepository.CreateAsync(classEntity);
            return _mapper.Map<ClassResponseDTO>(createdClass);
        }

        public async Task<ClassResponseDTO?> UpdateAsync(int id, ClassUpdateDTO dto)
        {
            var existingClass = await _classRepository.GetByIdAsync(id);
            if (existingClass == null) return null;

            var classWithCode = await _classRepository.GetByCodeAsync(dto.Code);
            if (classWithCode != null && classWithCode.Id != id)
            {
                throw new Exception("Já existe outra matéria com este código.");
            }

            existingClass.Code = dto.Code;
            existingClass.Name = dto.Name;
            existingClass.WorkloadHours = dto.WorkloadHours;
            existingClass.Status = dto.Status;

            var updatedClass = await _classRepository.UpdateAsync(existingClass);
            return updatedClass != null ? _mapper.Map<ClassResponseDTO>(updatedClass) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _classRepository.DeleteAsync(id);
        }
    }
}
