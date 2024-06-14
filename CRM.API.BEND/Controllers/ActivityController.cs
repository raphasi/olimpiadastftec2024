using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivityById(Guid id)
        {
            var activity = await _activityRepository.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetAllActivities()
        {
            var activities = await _activityRepository.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpPost]
        public async Task<ActionResult> AddActivity([FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            await _activityRepository.AddActivityAsync(activity);
            return CreatedAtAction(nameof(GetActivityById), new { id = activity.ActivityID }, activity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(Guid id, [FromBody] Activity activity)
        {
            if (activity == null || activity.ActivityID != id)
            {
                return BadRequest();
            }

            var existingActivity = await _activityRepository.GetActivityByIdAsync(id);
            if (existingActivity == null)
            {
                return NotFound();
            }

            await _activityRepository.UpdateActivityAsync(activity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            var existingActivity = await _activityRepository.GetActivityByIdAsync(id);
            if (existingActivity == null)
            {
                return NotFound();
            }

            await _activityRepository.DeleteActivityAsync(id);
            return NoContent();
        }
    }
}