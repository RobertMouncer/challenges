using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Models;
using Microsoft.AspNetCore.Authorization;
//Index page will be used to display group challenges that users can join. The create function will be used by the userChallenge to create a challenge for the user.
namespace challenges.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChallengesController : ControllerBase
    {
        private readonly challengesContext _context;

        public ChallengesController(challengesContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult NewChallenge([FromBody] Challenge challenge)
        {
            return StatusCode(501);
        }

        [HttpGet("find/{id:int}")]
        public IActionResult ListUserGroupChallenges([FromRoute] int id)
        {
            return StatusCode(501);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateChallenge([FromRoute] int id)
        {
            return StatusCode(501);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteChallenge([FromRoute] int id)
        {
            return StatusCode(501);
        }
    }
}
