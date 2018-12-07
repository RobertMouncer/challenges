using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ChallengesController : ControllerBase
    {
        private readonly ChallengesContext _context;

        public ChallengesController(ChallengesContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult NewChallenge([FromBody] Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Challenge.Add(challenge);
                return Ok(ModelState);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("find/{id:int}")]
        public IActionResult ListUserGroupChallenges([FromRoute] int? id)
        {
            if (id != null)
            {
                String sid = id.ToString();
                var challenges = _context.Challenge
                    .Include(c => c.Activity)
                    .Where(m => m.Groupid == sid);
                
                return Ok(challenges);
                
            }
            return BadRequest("ID was Null");
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateChallenge([FromRoute] int? id, [FromBody] Challenge challenge)
        {
            if (challenge.ChallengeId == id && ModelState.IsValid)
            {
                _context.Challenge.Update(challenge);

                return Ok(challenge);
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteChallenge([FromRoute] int? id)
        {
            if (id != null)
            {
                var challenge = _context.Challenge
                    .Include(c => c.Activity)
                    .FirstOrDefaultAsync(m => m.ChallengeId == id);

                if (challenge == null)
                    return NotFound();
                
                
            }
            return StatusCode(501);
        }
    }
}
