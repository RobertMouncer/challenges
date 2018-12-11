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
    [Route("api/challengesManage")]
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

            //userChallenge.Challenge.ActivityId = userChallenge.Challenge.Activity.ActivityId;

            userChallenge.Challenge.Activity = null;

            var challenge = await _challengeRepository.AddAsync(userChallenge.Challenge);

            userChallenge.Challenge = null;
            userChallenge.ChallengeId = challenge.ChallengeId;

            var user = await _userChallengeRepository.AddAsync(userChallenge);

            return Ok(user);
        }

        [HttpGet("getGroup/{uid}")]
        public async Task<IActionResult> ListUserGroupChallenges([FromRoute] string uid)
        {
            var userChallenges = await _userChallengeRepository.GetGroupByUid(uid);

            if (userChallenges == null)
                return Ok(new List<object>());
            
            return Ok(userChallenges);
        }

        [HttpGet("getPersonal/{uid}")]
        public async Task<IActionResult> ListPersonalChallenges([FromRoute] string uid)
        {
            var userChallenges = await _userChallengeRepository.GetAllPersonalChallenges(uid);

            if (userChallenges == null)
                return Ok(new List<object>());

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

            userChallenge.Challenge.Activity = null;
            await _userChallengeRepository.UpdateAsync(userChallenge);

            return Ok(userChallenge);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserChallenge([FromRoute] int id)
        {
            var userChallenge = await _userChallengeRepository.GetByIdAsync(id);
            if (userChallenge == null)
                return NotFound();

            userChallenge.Challenge.Activity = null;
            await _challengeRepository.DeleteAsync(userChallenge.Challenge);
            return Ok(userChallenge);
        }

        private async Task ValidateUserChallenge(UserChallenge userChallenge)
        {
            if (userChallenge.Challenge.IsGroupChallenge)
                ModelState.AddModelError("IsGroupChallenge", "Invalid userChallenge, must be a personal challenge.");

            var challenge = userChallenge.Challenge;
            var activityId = challenge.Activity != null ? challenge.Activity.ActivityId : challenge.ActivityId;

            var activity = await _activityRepository.FindByIdAsync(activityId);
            if (activity == null)
                ModelState.AddModelError("activityId", "Invalid Activity id received, activity doesn't exist.");
        }
    }
}
