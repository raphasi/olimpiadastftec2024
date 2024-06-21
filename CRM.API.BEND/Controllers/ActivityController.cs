using CRM.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using CRM.Application.Interfaces;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDTO>> GetActivityById(Guid id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetAllActivities()
        {
            var activities = await _activityService.GetAllAsync();
            return Ok(activities);
        }

        [HttpPost]
        public async Task<ActionResult> AddActivity([FromBody] ActivityDTO activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            await _activityService.AddAsync(activity);
            return CreatedAtAction(nameof(GetActivityById), new { id = activity.ActivityID }, activity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(Guid id, [FromBody] ActivityDTO activity)
        {
            if (activity == null || activity.ActivityID != id)
            {
                return BadRequest();
            }

            var existingActivity = await _activityService.GetByIdAsync(id);
            if (existingActivity == null)
            {
                return NotFound();
            }

            await _activityService.UpdateAsync(activity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            var existingActivity = await _activityService.GetByIdAsync(id);
            if (existingActivity == null)
            {
                return NotFound();
            }

            await _activityService.DeleteAsync(id);
            return NoContent();
        }
    }
}