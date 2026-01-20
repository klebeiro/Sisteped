using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Models.Enums;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class ClassTeacherService : IClassTeacherService
    {
        private readonly IClassTeacherRepository _classTeacherRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ClassTeacherService(
            IClassTeacherRepository classTeacherRepository,
            IClassRepository classRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _classTeacherRepository = classTeacherRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ClassTeacherResponseDTO?> GetByIdAsync(int id)
        {
            var classTeacher = await _classTeacherRepository.GetByIdAsync(id);
            return classTeacher != null ? _mapper.Map<ClassTeacherResponseDTO>(classTeacher) : null;
        }

        public async Task<IEnumerable<ClassTeacherResponseDTO>> GetAllAsync()
        {
            var classTeachers = await _classTeacherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClassTeacherResponseDTO>>(classTeachers);
        }

        public async Task<IEnumerable<ClassTeacherResponseDTO>> GetByClassIdAsync(int classId)
        {
            var classTeachers = await _classTeacherRepository.GetByClassIdAsync(classId);
            return _mapper.Map<IEnumerable<ClassTeacherResponseDTO>>(classTeachers);
        }

        public async Task<IEnumerable<ClassTeacherResponseDTO>> GetByTeacherIdAsync(int teacherId)
        {
            var classTeachers = await _classTeacherRepository.GetByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<ClassTeacherResponseDTO>>(classTeachers);
        }

        public async Task<ClassTeacherResponseDTO> CreateAsync(ClassTeacherDTO dto)
        {
            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            var teacher = await _userRepository.GetByIdAsync(dto.TeacherId);
            if (teacher == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            if (teacher.Role != UserRole.Teacher && teacher.Role != UserRole.Coordinator)
            {
                throw new Exception("O usuário informado não é um professor.");
            }

            if (await _classTeacherRepository.ExistsAsync(dto.ClassId, dto.TeacherId))
            {
                throw new Exception("Este professor já está associado a esta matéria.");
            }

            var classTeacher = new ClassTeacher
            {
                ClassId = dto.ClassId,
                TeacherId = dto.TeacherId
            };

            var createdClassTeacher = await _classTeacherRepository.CreateAsync(classTeacher);
            return _mapper.Map<ClassTeacherResponseDTO>(createdClassTeacher);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _classTeacherRepository.DeleteAsync(id);
        }
    }
}
