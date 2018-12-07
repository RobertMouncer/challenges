using System.Net;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace challenges.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ChallengesContext _context;
        
        public ActivitiesController(ChallengesContext ctx)
        {
            _context = ctx;
        }
        
        [HttpGet]
        public IActionResult GetActivities()
        {
            //TODO Implement functionality.
            return StatusCode(501);
        }
    }
}