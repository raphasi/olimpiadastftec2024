using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IActivityRepository
{
    Task<Activity> GetActivityByIdAsync(Guid activityId);
    Task<IEnumerable<Activity>> GetAllActivitiesAsync();
    Task AddActivityAsync(Activity activityEntity);
    Task UpdateActivityAsync(Activity activityEntity);
    Task DeleteActivityAsync(Guid activityId);
}