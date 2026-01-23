using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ActivityService(
            IActivityRepository activityRepository,
            IClassRepository classRepository,
            IMapper mapper)
        {
            _activityRepository = activityRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ActivityResponseDTO?> GetByIdAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            return activity != null ? _mapper.Map<ActivityResponseDTO>(activity) : null;
        }

        public async Task<IEnumerable<ActivityResponseDTO>> GetAllAsync()
        {
            var activities = await _activityRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ActivityResponseDTO>>(activities);
        }

        public async Task<IEnumerable<ActivityResponseDTO>> GetByClassIdAsync(int classId)
        {
            var activities = await _activityRepository.GetByClassIdAsync(classId);
            return _mapper.Map<IEnumerable<ActivityResponseDTO>>(activities);
        }

        public async Task<IEnumerable<ActivityResponseDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var activities = await _activityRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<ActivityResponseDTO>>(activities);
        }

        public async Task<ActivityResponseDTO> CreateAsync(ActivityCreateDTO dto)
        {
            if (!await _classRepository.ExistsAsync(dto.ClassId))
            {
                throw new Exception("Matéria não encontrada.");
            }

            if (dto.MaxScore <= 0)
            {
                throw new Exception("A nota máxima deve ser maior que zero.");
            }

            var activity = new Activity
            {
                Title = dto.Title,
                Description = dto.Description,
                ClassId = dto.ClassId,
                ApplicationDate = dto.ApplicationDate,
                MaxScore = dto.MaxScore
            };

            var createdActivity = await _activityRepository.CreateAsync(activity);
            return _mapper.Map<ActivityResponseDTO>(createdActivity);
        }

        public async Task<ActivityResponseDTO?> UpdateAsync(int id, ActivityUpdateDTO dto)
        {
            var existingActivity = await _activityRepository.GetByIdAsync(id);
            if (existingActivity == null)
            {
                return null;
            }

            if (dto.MaxScore <= 0)
            {
                throw new Exception("A nota máxima deve ser maior que zero.");
            }

            existingActivity.Title = dto.Title;
            existingActivity.Description = dto.Description;
            existingActivity.ApplicationDate = dto.ApplicationDate;
            existingActivity.MaxScore = dto.MaxScore;
            existingActivity.Status = dto.Status;

            var updatedActivity = await _activityRepository.UpdateAsync(existingActivity);
            return updatedActivity != null ? _mapper.Map<ActivityResponseDTO>(updatedActivity) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _activityRepository.DeleteAsync(id);
        }
    }
}
