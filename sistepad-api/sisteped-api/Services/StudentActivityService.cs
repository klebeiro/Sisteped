using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class StudentActivityService : IStudentActivityService
    {
        private readonly IStudentActivityRepository _studentActivityRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public StudentActivityService(
            IStudentActivityRepository studentActivityRepository,
            IStudentRepository studentRepository,
            IActivityRepository activityRepository,
            IMapper mapper)
        {
            _studentActivityRepository = studentActivityRepository;
            _studentRepository = studentRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<StudentActivityResponseDTO?> GetByIdAsync(int id)
        {
            var studentActivity = await _studentActivityRepository.GetByIdAsync(id);
            return studentActivity != null ? _mapper.Map<StudentActivityResponseDTO>(studentActivity) : null;
        }

        public async Task<IEnumerable<StudentActivityResponseDTO>> GetAllAsync()
        {
            var studentActivities = await _studentActivityRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentActivityResponseDTO>>(studentActivities);
        }

        public async Task<IEnumerable<StudentActivityResponseDTO>> GetByStudentIdAsync(int studentId)
        {
            var studentActivities = await _studentActivityRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentActivityResponseDTO>>(studentActivities);
        }

        public async Task<IEnumerable<StudentActivityResponseDTO>> GetByActivityIdAsync(int activityId)
        {
            var studentActivities = await _studentActivityRepository.GetByActivityIdAsync(activityId);
            return _mapper.Map<IEnumerable<StudentActivityResponseDTO>>(studentActivities);
        }

        public async Task<StudentActivityResponseDTO> CreateAsync(StudentActivityCreateDTO dto)
        {
            if (!await _studentRepository.ExistsAsync(dto.StudentId))
            {
                throw new Exception("Aluno não encontrado.");
            }

            var activity = await _activityRepository.GetByIdAsync(dto.ActivityId);
            if (activity == null)
            {
                throw new Exception("Atividade não encontrada.");
            }

            if (await _studentActivityRepository.ExistsAsync(dto.StudentId, dto.ActivityId))
            {
                throw new Exception("Já existe registro de nota para este aluno nesta atividade.");
            }

            if (dto.Score.HasValue && dto.Score.Value > activity.MaxScore)
            {
                throw new Exception($"A nota não pode ser maior que a nota máxima da atividade ({activity.MaxScore}).");
            }

            if (dto.Score.HasValue && dto.Score.Value < 0)
            {
                throw new Exception("A nota não pode ser negativa.");
            }

            var studentActivity = new StudentActivity
            {
                StudentId = dto.StudentId,
                ActivityId = dto.ActivityId,
                Score = dto.Score,
                Remarks = dto.Remarks
            };

            var createdStudentActivity = await _studentActivityRepository.CreateAsync(studentActivity);
            return _mapper.Map<StudentActivityResponseDTO>(createdStudentActivity);
        }

        public async Task<IEnumerable<StudentActivityResponseDTO>> CreateBulkAsync(StudentActivityBulkCreateDTO dto)
        {
            var activity = await _activityRepository.GetByIdAsync(dto.ActivityId);
            if (activity == null)
            {
                throw new Exception("Atividade não encontrada.");
            }

            // Validar todos os alunos
            var studentIds = dto.Scores.Select(s => s.StudentId).Distinct().ToList();
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

            // Validar notas
            var invalidScores = dto.Scores.Where(s => s.Score.HasValue && (s.Score.Value > activity.MaxScore || s.Score.Value < 0)).ToList();
            if (invalidScores.Any())
            {
                throw new Exception($"Notas inválidas encontradas. As notas devem estar entre 0 e {activity.MaxScore}.");
            }

            var studentActivities = new List<StudentActivity>();

            foreach (var studentScore in dto.Scores)
            {
                // Verifica se já existe registro para este aluno nesta atividade
                if (await _studentActivityRepository.ExistsAsync(studentScore.StudentId, dto.ActivityId))
                {
                    continue;
                }

                studentActivities.Add(new StudentActivity
                {
                    StudentId = studentScore.StudentId,
                    ActivityId = dto.ActivityId,
                    Score = studentScore.Score,
                    Remarks = studentScore.Remarks
                });
            }

            if (!studentActivities.Any())
            {
                return Enumerable.Empty<StudentActivityResponseDTO>();
            }

            var createdStudentActivities = await _studentActivityRepository.CreateBulkAsync(studentActivities);
            return _mapper.Map<IEnumerable<StudentActivityResponseDTO>>(createdStudentActivities);
        }

        public async Task<StudentActivityResponseDTO?> UpdateAsync(int id, StudentActivityUpdateDTO dto)
        {
            var existingStudentActivity = await _studentActivityRepository.GetByIdAsync(id);
            if (existingStudentActivity == null)
            {
                return null;
            }

            var activity = existingStudentActivity.Activity;
            
            if (dto.Score.HasValue && dto.Score.Value > activity.MaxScore)
            {
                throw new Exception($"A nota não pode ser maior que a nota máxima da atividade ({activity.MaxScore}).");
            }

            if (dto.Score.HasValue && dto.Score.Value < 0)
            {
                throw new Exception("A nota não pode ser negativa.");
            }

            existingStudentActivity.Score = dto.Score;
            existingStudentActivity.Remarks = dto.Remarks;

            var updatedStudentActivity = await _studentActivityRepository.UpdateAsync(existingStudentActivity);
            return updatedStudentActivity != null ? _mapper.Map<StudentActivityResponseDTO>(updatedStudentActivity) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _studentActivityRepository.DeleteAsync(id);
        }
    }
}
