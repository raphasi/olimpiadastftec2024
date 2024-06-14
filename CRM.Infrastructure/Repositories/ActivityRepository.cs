using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _activityContext;

        public ActivityRepository(ApplicationDbContext activityContext)
        {
            _activityContext = activityContext;
        }

        public async Task<Activity> GetActivityByIdAsync(Guid activityId)
        {
            return await _activityContext.Set<Activity>().FindAsync(activityId);
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityContext.Set<Activity>().ToListAsync();
        }

        public async Task AddActivityAsync(Activity activityEntity)
        {
            await _activityContext.Set<Activity>().AddAsync(activityEntity);
            await _activityContext.SaveChangesAsync();
        }

        public async Task UpdateActivityAsync(Activity activityEntity)
        {
            _activityContext.Set<Activity>().Update(activityEntity);
            await _activityContext.SaveChangesAsync();
        }

        public async Task DeleteActivityAsync(Guid activityId)
        {
            var activity = await _activityContext.Set<Activity>().FindAsync(activityId);
            if (activity != null)
            {
                _activityContext.Set<Activity>().Remove(activity);
                await _activityContext.SaveChangesAsync();
            }
        }
    }
}