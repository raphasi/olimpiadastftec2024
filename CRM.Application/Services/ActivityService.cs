using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDTO>> GetAllAsync()
        {
            var activities = await _activityRepository.GetAllActivitiesAsync();
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        public async Task<ActivityDTO> GetByIdAsync(Guid id)
        {
            var activity = await _activityRepository.GetActivityByIdAsync(id);
            return _mapper.Map<ActivityDTO>(activity);
        }

        public async Task<ActivityDTO> AddAsync(ActivityDTO activity)
        {
            activity.ActivityID = Guid.NewGuid();   
            var activityEntity = _mapper.Map<Activity>(activity);
            await _activityRepository.AddActivityAsync(activityEntity);
            return activity;
        }

        public async Task UpdateAsync(ActivityDTO activity)
        {
            var activityEntity = _mapper.Map<Activity>(activity);
            await _activityRepository.UpdateActivityAsync(activityEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _activityRepository.DeleteActivityAsync(id);
        }
    }
}