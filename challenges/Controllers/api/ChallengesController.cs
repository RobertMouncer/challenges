using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Models;
using challenges.Repositories;
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
        private readonly IChallengeRepository challengeRepository;
        private readonly IUserChallengeRepository userChallengeRepository;

        public ChallengesController(IChallengeRepository challengeRepository, IUserChallengeRepository userChallengeRepository)
        {
            this.challengeRepository = challengeRepository;
            this.userChallengeRepository = userChallengeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> NewChallenge([FromBody] UserChallenge userChallenge)
        {
            //TODO Possibly add some validation
            
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await userChallengeRepository.AddAsync(userChallenge);
            await challengeRepository.AddAsync(userChallenge.Challenge);

            return Ok(userChallenge);
        }

        [HttpGet("find/{id:int}")]
        public async Task<IActionResult> ListUserGroupChallenges([FromRoute] int id)
        {
            String sid = id.ToString();
            var userChallenges = await userChallengeRepository.GetByGroupIdAsync(sid);

            if (userChallenges == null)
                return NotFound();
            
            return Ok(userChallenges);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateChallenge([FromRoute] int id, [FromBody] UserChallenge userChallenge)
        {
            if (userChallenge.UserChallengeId != id)
            {
                ModelState.AddModelError("Id", "Id must match URL parameter.");
            }
            
            //TODO Possibly add some validation

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await userChallengeRepository.UpdateAsync(userChallenge);

            return Ok(userChallenge);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteChallenge([FromRoute] int id)
        {
            var userChallenge = await userChallengeRepository.GetByIdAsync(id);
            if (userChallenge == null)
                return NotFound();

            await userChallengeRepository.DeleteAsync(userChallenge);
            return Ok(userChallenge);
        }
    }
}
