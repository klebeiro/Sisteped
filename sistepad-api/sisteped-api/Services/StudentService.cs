using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Models.Enums;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public StudentService(
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<StudentResponseDTO?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student != null ? _mapper.Map<StudentResponseDTO>(student) : null;
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentResponseDTO>>(students);
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetByGuardianIdAsync(int guardianId)
        {
            var students = await _studentRepository.GetByGuardianIdAsync(guardianId);
            return _mapper.Map<IEnumerable<StudentResponseDTO>>(students);
        }

        public async Task<StudentResponseDTO> CreateAsync(StudentCreateDTO dto)
        {
            var existingStudent = await _studentRepository.GetByEnrollmentAsync(dto.Enrollment);
            if (existingStudent != null)
            {
                throw new Exception("Já existe um aluno com esta matrícula.");
            }

            var guardian = await _userRepository.GetByIdAsync(dto.GuardianId);
            if (guardian == null)
            {
                throw new Exception("Responsável não encontrado.");
            }

            if (guardian.Role != UserRole.Guardian)
            {
                throw new Exception("O usuário informado não é um responsável.");
            }

            var student = _mapper.Map<Student>(dto);
            var createdStudent = await _studentRepository.CreateAsync(student);
            return _mapper.Map<StudentResponseDTO>(createdStudent);
        }

        public async Task<StudentResponseDTO?> UpdateAsync(int id, StudentUpdateDTO dto)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null) return null;

            var studentWithEnrollment = await _studentRepository.GetByEnrollmentAsync(dto.Enrollment);
            if (studentWithEnrollment != null && studentWithEnrollment.Id != id)
            {
                throw new Exception("Já existe outro aluno com esta matrícula.");
            }

            var guardian = await _userRepository.GetByIdAsync(dto.GuardianId);
            if (guardian == null)
            {
                throw new Exception("Responsável não encontrado.");
            }

            if (guardian.Role != UserRole.Guardian)
            {
                throw new Exception("O usuário informado não é um responsável.");
            }

            existingStudent.Enrollment = dto.Enrollment;
            existingStudent.Name = dto.Name;
            existingStudent.BirthDate = dto.BirthDate;
            existingStudent.GuardianId = dto.GuardianId;
            existingStudent.Status = dto.Status;

            var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
            return updatedStudent != null ? _mapper.Map<StudentResponseDTO>(updatedStudent) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _studentRepository.DeleteAsync(id);
        }
    }
}
