using System.Collections.Generic;
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
    public class GoalMetricController : ControllerBase
    {
        private readonly IGoalMetricRepository GoalMetricRepository;
        
        public GoalMetricController(IGoalMetricRepository GoalMetricRepository)
        {
            this.GoalMetricRepository = GoalMetricRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetGoalMetrics()
        {
            var goalMetrics = await GoalMetricRepository.GetAllAsync();
            if (goalMetrics == null)
                return Ok(new List<object>());
            return Ok(goalMetrics);
        }
    }
}