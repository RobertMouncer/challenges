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
using YourApp.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//display all user challenges.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc")]
    public class UserChallengesController : Controller
    {
        private readonly IUserChallengeRepository _userChallengeRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IApiClient client;

        public UserChallengesController(IUserChallengeRepository userChallengeRepository, IChallengeRepository challengeRepository, IApiClient client)
        {
            _userChallengeRepository = userChallengeRepository;
            _challengeRepository = challengeRepository;
            this.client = client;
        }

        // GET: UserChallenges
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            if (isAdminOrCoord())
            {
                var challengesContext = _userChallengeRepository.GetAll();
                List<string> userList = new List<string>();

                foreach (UserChallenge u in challengesContext)
                {
                    userList.Add(u.UserId);
                }
                
                var response = await client.PostAsync("https://docker2.aberfitness.biz/gatekeeper/api/Users/Batch", userList.Distinct());
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

                return View(await challengesContext.ToListAsync());
            } else
            {
                var challengesContext = _userChallengeRepository.GetByUId(userId);

                foreach(var c in challengesContext)
                {
                    var userData = await client.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/Activities/ByUser/" 
                                                        + c.UserId + "?from=" + c.Challenge.StartDateTime.Date + "&to=" + c.Challenge.EndDateTime);

                    var userDataResult = userData.Content.ReadAsStringAsync().Result;
                    
                    UpdatePercentageCompleteAsync(c, userDataResult);
                }
                

                return View(await challengesContext.ToListAsync());
            }
            

        }


        // GET: UserChallenges/Create
        public IActionResult Create()
        {
            ViewData["ChallengeId"] = new SelectList(_challengeRepository.GetDBSet(), "ChallengeId", "ChallengeId");
            return View();
        }

        // POST: UserChallenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserChallengeId,UserId,ChallengeId,PercentageComplete")] UserChallenge userChallenge)
        {
            if (ModelState.IsValid)
            {
                await _userChallengeRepository.AddAsync(userChallenge);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_challengeRepository.GetDBSet(), "ChallengeId", "ChallengeId", userChallenge.ChallengeId);
            return View(userChallenge);
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

            return View(userChallenge);
        }

        // POST: UserChallenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userChallenge = await _userChallengeRepository.FindByIdAsync(id);
            await _userChallengeRepository.DeleteAsync(userChallenge);
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

        private async Task<UserChallenge> UpdatePercentageCompleteAsync(UserChallenge userChallenge, string userDataString)
        {
            dynamic dataString = JsonConvert.DeserializeObject(userDataString);
            var progress = 0;

            foreach (var d in dataString)
            {
                if (d.activityTypeId == userChallenge.Challenge.ActivityId)
                {
                    switch (userChallenge.Challenge.GoalMetric)
                    {
                        case "caloriesBurnt":
                            progress += d.caloriesBurnt;
                            break;
                        case "averageHeartRate":
                            progress += d.averageHeartRate;
                            break;
                        case "stepsTaken":
                            progress += d.stepsTaken;
                            break;
                        case "metresTravelled":
                            progress += d.metresTravelled;
                            break;
                        case "metresElevationGained":
                            progress += d.metresElevationGained;
                            break;
                    }
                }
            }

            var percentageComplete = progress / userChallenge.Challenge.Goal;
            userChallenge.PercentageComplete = percentageComplete;

            await _userChallengeRepository.UpdateAsync(userChallenge);

            return userChallenge;
        }
    }
}
