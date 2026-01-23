using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IStudentRepository studentRepository,
            IClassRepository classRepository,
            IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceResponseDTO?> GetByIdAsync(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            return attendance != null ? _mapper.Map<AttendanceResponseDTO>(attendance) : null;
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetAllAsync()
        {
            var attendances = await _attendanceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetByStudentIdAsync(int studentId)
        {
            var attendances = await _attendanceRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetByClassIdAsync(int classId)
        {
            var attendances = await _attendanceRepository.GetByClassIdAsync(classId);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetByDateAsync(DateTime date)
        {
            var attendances = await _attendanceRepository.GetByDateAsync(date);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetByClassAndDateAsync(int classId, DateTime date)
        {
            var attendances = await _attendanceRepository.GetByClassAndDateAsync(classId, date);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetByStudentAndClassAsync(int studentId, int classId)
        {
            var attendances = await _attendanceRepository.GetByStudentAndClassAsync(studentId, classId);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(attendances);
        }

        public async Task<AttendanceResponseDTO> CreateAsync(AttendanceCreateDTO dto)
        {
            if (!await _studentRepository.ExistsAsync(dto.StudentId))
            {
                throw new Exception("Aluno não encontrado.");
            }

            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            if (await _attendanceRepository.ExistsAsync(dto.StudentId, dto.ClassId, dto.Date))
            {
                throw new Exception("Já existe registro de frequência para este aluno nesta matéria e data.");
            }

            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Date = dto.Date,
                Present = dto.Present
            };

            var createdAttendance = await _attendanceRepository.CreateAsync(attendance);
            return _mapper.Map<AttendanceResponseDTO>(createdAttendance);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> CreateBulkAsync(AttendanceBulkCreateDTO dto)
        {
            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            // Validar todos os alunos de uma vez
            var studentIds = dto.Students.Select(s => s.StudentId).Distinct().ToList();
            var invalidStudentIds = new List<int>();
            
            foreach (var studentId in studentIds)
            {
                if (!await _studentRepository.ExistsAsync(studentId))
                {
                    invalidStudentIds.Add(studentId);
                }
            }

            if (invalidStudentIds.Any())
            {
                throw new Exception($"Aluno(s) com ID(s) {string.Join(", ", invalidStudentIds)} não encontrado(s).");
            }

            var attendances = new List<Attendance>();

            foreach (var studentAttendance in dto.Students)
            {
                // Verifica se já existe registro para este aluno nesta matéria e data
                if (await _attendanceRepository.ExistsAsync(studentAttendance.StudentId, dto.ClassId, dto.Date))
                {
                    continue;
                }

                attendances.Add(new Attendance
                {
                    StudentId = studentAttendance.StudentId,
                    ClassId = dto.ClassId,
                    Date = dto.Date,
                    Present = studentAttendance.Present
                });
            }

            if (!attendances.Any())
            {
                return Enumerable.Empty<AttendanceResponseDTO>();
            }

            var createdAttendances = await _attendanceRepository.CreateBulkAsync(attendances);
            return _mapper.Map<IEnumerable<AttendanceResponseDTO>>(createdAttendances);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _attendanceRepository.DeleteAsync(id);
        }
    }
}
