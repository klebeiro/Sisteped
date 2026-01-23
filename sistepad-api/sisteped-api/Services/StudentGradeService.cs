using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class StudentGradeService : IStudentGradeService
    {
        private readonly IStudentGradeRepository _studentGradeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;

        public StudentGradeService(
            IStudentGradeRepository studentGradeRepository,
            IStudentRepository studentRepository,
            IGradeRepository gradeRepository,
            IMapper mapper)
        {
            _studentGradeRepository = studentGradeRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _mapper = mapper;
        }

        public async Task<StudentGradeResponseDTO?> GetByIdAsync(int id)
        {
            var studentGrade = await _studentGradeRepository.GetByIdAsync(id);
            return studentGrade != null ? _mapper.Map<StudentGradeResponseDTO>(studentGrade) : null;
        }

        public async Task<IEnumerable<StudentGradeResponseDTO>> GetAllAsync()
        {
            var studentGrades = await _studentGradeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentGradeResponseDTO>>(studentGrades);
        }

        public async Task<IEnumerable<StudentGradeResponseDTO>> GetByStudentIdAsync(int studentId)
        {
            var studentGrades = await _studentGradeRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentGradeResponseDTO>>(studentGrades);
        }

        public async Task<IEnumerable<StudentGradeResponseDTO>> GetByGradeIdAsync(int gradeId)
        {
            var studentGrades = await _studentGradeRepository.GetByGradeIdAsync(gradeId);
            return _mapper.Map<IEnumerable<StudentGradeResponseDTO>>(studentGrades);
        }

        public async Task<StudentGradeResponseDTO> CreateAsync(StudentGradeDTO dto)
        {
            if (!await _studentRepository.ExistsAsync(dto.StudentId))
            {
                throw new Exception("Aluno não encontrado.");
            }

            if (!await _gradeRepository.ExistsAsync(dto.GradeId))
            {
                throw new Exception("Turma não encontrada.");
            }

            if (await _studentGradeRepository.ExistsAsync(dto.StudentId, dto.GradeId))
            {
                throw new Exception("Este aluno já está vinculado a esta turma.");
            }

            var studentGrade = new StudentGrade
            {
                StudentId = dto.StudentId,
                GradeId = dto.GradeId
            };

            var createdStudentGrade = await _studentGradeRepository.CreateAsync(studentGrade);
            return _mapper.Map<StudentGradeResponseDTO>(createdStudentGrade);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _studentGradeRepository.DeleteAsync(id);
        }
    }
}
