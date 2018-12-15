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
using Microsoft.Extensions.Configuration;
using YourApp.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AberFitnessAuditLogger;
//display all user challenges.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc")]
    public class UserChallengesController : Controller
    {
        private readonly IUserChallengeRepository _userChallengeRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IApiClient client;
        private readonly IConfigurationSection _appConfig;
        private readonly IAuditLogger auditLogger;

        public UserChallengesController(IUserChallengeRepository userChallengeRepository, IChallengeRepository challengeRepository, 
            IApiClient client, IConfiguration config, IAuditLogger auditLogger)
        {
            _userChallengeRepository = userChallengeRepository;
            _challengeRepository = challengeRepository;
            this.client = client;
            _appConfig = config.GetSection("Challenges");
            this.auditLogger = auditLogger;
        }

        // GET: UserChallenges
        public async Task<IActionResult> Index()
        {
            var userId = getUserId();


            await auditLogger.log(getUserId(), $"Accessed User Challenged Index");
            if (isAdminOrCoord())
            {
                var challengesContext = await _userChallengeRepository.GetAllAsync();

                List<string> userList = new List<string>();

                foreach (UserChallenge u in challengesContext)
                {
                    userList.Add(u.UserId);
                }
                
                var response = await client.PostAsync(_appConfig.GetValue<string>("GatekeeperUrl") + "api/Users/Batch", userList.Distinct());
                JArray jsonArrayOfUsers = JArray.Parse(await response.Content.ReadAsStringAsync());

                foreach (UserChallenge u in challengesContext)
                {
                    foreach (JObject j in jsonArrayOfUsers)
                    {
                        if (u.UserId == j.GetValue("id").ToString())
                        {
                            u.UserId = j.GetValue("email").ToString();
                        }
                    }
                }
                return View(challengesContext);
            } else

            {

                var challengesContext = await _userChallengeRepository.GetByUId(userId);

                return View(challengesContext);
            }
            

        }

        // GET: UserChallenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChallenge = await _userChallengeRepository.GetByIdAsync((int) id);
            if (userChallenge == null)
            {
                return NotFound();
            }
            await auditLogger.log(getUserId(), $"Accessed Delete User Challenge: {id}");
            return View(userChallenge);
        }

        // POST: UserChallenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userChallenge = await _userChallengeRepository.GetByIdAsync(id);
            await _userChallengeRepository.DeleteAsync(userChallenge);
            await auditLogger.log(getUserId(), $"Deleted User Challenge: {id}");
            return RedirectToAction(nameof(Index));
        }

        private bool UserChallengeExists(int id)
        {
            return _userChallengeRepository.Exists(id);
        }

        public bool isAdminOrCoord()
        {
            return (User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("coordinator") || User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("administrator"));
        }

        private string getUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
        }
    }
}
