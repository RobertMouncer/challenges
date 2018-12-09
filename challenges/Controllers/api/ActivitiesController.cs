using System.Net;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using challenges.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace challenges.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository activityRepository;
        
        public ActivitiesController(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var activities = await activityRepository.GetAllAsync();
            if (activities == null)
                return NotFound();
            return Ok(activities);
        }
    }
}