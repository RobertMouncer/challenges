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

namespace challenges.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChallengesController : ControllerBase
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IUserChallengeRepository _userChallengeRepository;
        private readonly IActivityRepository _activityRepository;

        public ChallengesController(IChallengeRepository challengeRepository, 
            IUserChallengeRepository userChallengeRepository, IActivityRepository activityRepository)
        {
            _challengeRepository = challengeRepository;
            _userChallengeRepository = userChallengeRepository;
            _activityRepository = activityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> NewChallenge([FromBody] UserChallenge userChallenge)
        {
            await ValidateUserChallenge(userChallenge);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _userChallengeRepository.AddAsync(userChallenge);

            return Ok(userChallenge);
        }

        [HttpGet("find/{ugid}")]
        public async Task<IActionResult> ListUserGroupChallenges([FromRoute] string ugid)
        {
            var userChallenges = await _userChallengeRepository.GetByGroupIdAsync(ugid);

            if (userChallenges == null)
                return NotFound();
            
            return Ok(userChallenges);
        }

        [HttpGet("fromUser/{uid}")]
        public async Task<IActionResult> ListPersonalChallenges([FromRoute] string uid)
        {
            var userChallenges = await _userChallengeRepository.GetAllPersonalChallenges(uid);

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

            await ValidateUserChallenge(userChallenge);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userChallengeRepository.UpdateAsync(userChallenge);

            return Ok(userChallenge);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserChallenge([FromRoute] int id)
        {
            var userChallenge = await _userChallengeRepository.GetByIdAsync(id);
            if (userChallenge == null)
                return NotFound();

            await _challengeRepository.DeleteAsync(userChallenge.Challenge);
            //await _userChallengeRepository.DeleteAsync(userChallenge);
            return Ok(userChallenge);
        }

        private async Task ValidateUserChallenge(UserChallenge userChallenge)
        {
            if (userChallenge.Challenge.IsGroupChallenge)
                ModelState.AddModelError("IsGroupChallenge", "Invalid userChallenge, must be a personal challenge.");

            var activity = await _activityRepository.GetByIdAsync(userChallenge.Challenge.Activity.ActivityId);
            if (activity == null)
                ModelState.AddModelError("activityId", "Invalid Activity id received, activity doesn't exist.");
        }
    }
}
