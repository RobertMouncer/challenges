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
using Newtonsoft.Json;
//Index page will be used to display group challenges that users can join. The create function will be used by the userChallenge to create a challenge for the user.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc")]
    public class ChallengesManageController : Controller
    {
        private readonly IUserChallengeRepository _userChallengeRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IGoalMetricRepository _goalMetricRepository;
        private readonly IApiClient client;
        private readonly IConfigurationSection _appConfig;

        public ChallengesManageController(IUserChallengeRepository userChallengeRepository, IChallengeRepository challengeRepository, 
            IActivityRepository activityRepository, IGoalMetricRepository goalMetricRepository, IApiClient client, IConfiguration config)
        {
            _goalMetricRepository = goalMetricRepository;
            _userChallengeRepository = userChallengeRepository;
            _challengeRepository = challengeRepository;
            _activityRepository = activityRepository;
            this.client = client;
            _appConfig = config.GetSection("Challenges");
        }

        
        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            
            if (isAdminOrCoord())
            {
                var challengesContext = await _challengeRepository.GetAllGroup();

                foreach(var c in challengesContext)
                {
                    var groupResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups/" + c.Groupid);
                    var group = groupResponse.Content.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject(group);
                    c.Groupid = data.name;
                }

                return View(challengesContext);
            }
            else
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                var groupResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups/ForUser/" + userId);
                var group = groupResponse.Content.ReadAsStringAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(group);
                string groupId;
                if (data != null)
                    groupId = data.id;
                else
                    groupId = "NOTHING";
                 //TODO get user group and only display for that group
                 var challengesContext = _challengeRepository.GetAllByGroupId(groupId);
                return View(await challengesContext);
            }
            

        }

        // GET: Challenges/Create
        public async Task<IActionResult> Create()
        {
            var groupsResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups");
            var groups = groupsResponse.Content.ReadAsStringAsync().Result;
            var items = GetGroups(groups);


            ViewData["GoalMetricId"] = new SelectList(await _goalMetricRepository.GetAllAsync(), "GoalMetricId", "GoalMetricDisplay");
            ViewData["ActivityId"] = new SelectList(_activityRepository.GetDBSet(), "ActivityId", "ActivityName");
            ViewData["Groupid"] = new SelectList(items, "Value", "Text");
            return View();
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChallengeId,StartDateTime,EndDateTime,Goal,GoalMetricId,ActivityId,IsGroupChallenge,Groupid")] Challenge challenge)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            if (DateTime.Compare(challenge.StartDateTime.Date, DateTime.Now.Date) < 0)
            {
                ModelState.AddModelError("StartDateTime", "Date/Time is in the past. Please enter future Date or todays date.");
            }
            if (DateTime.Compare(challenge.EndDateTime, challenge.StartDateTime) <= 0)
            {
                ModelState.AddModelError("EndDateTime", "End Date/Time should be after the start Date/Time. Please re-enter Date/Time.");
            }

            UserChallenge user = new UserChallenge
            {
                UserId = userId,
                Challenge = challenge,
                ChallengeId = challenge.ChallengeId
            };

            if (ModelState.IsValid)
            {
                
                await _challengeRepository.AddAsync(challenge);
                if (!challenge.IsGroupChallenge)
                {
                    await _userChallengeRepository.AddAsync(user);
                }

                if (challenge.IsGroupChallenge)
                {
                    return RedirectToAction("Index", "ChallengesManage");
                }
                return RedirectToAction("Index", "UserChallenges");
            }

            var groupsResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups");
            var groups = groupsResponse.Content.ReadAsStringAsync().Result;
            var items = GetGroups(groups);

            ViewData["GoalMetricId"] = new SelectList(await _goalMetricRepository.GetAllAsync(), "GoalMetricId", "GoalMetricDisplay");
            ViewData["ActivityId"] = new SelectList(_activityRepository.GetDBSet(), "ActivityId", "ActivityName");
            ViewData["Groupid"] = new SelectList(items, "Value", "Text");
            return View(challenge);
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _challengeRepository.FindByIdAsync((int) id);
            if (challenge == null)
            {
                return NotFound();
            }

            var groupsResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups");
            var groups = groupsResponse.Content.ReadAsStringAsync().Result;
            var items = GetGroups(groups);

            ViewData["GoalMetricId"] = new SelectList(await _goalMetricRepository.GetAllAsync(), "GoalMetricId", "GoalMetricDisplay");

            ViewData["ActivityId"] = new SelectList(_activityRepository.GetDBSet(), "ActivityId", "ActivityName");
            ViewData["Groupid"] = new SelectList(items, "Value", "Text");
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChallengeId,StartDateTime,EndDateTime,Goal,GoalMetricId,ActivityId,IsGroupChallenge,Groupid")] Challenge challenge)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            UserChallenge user = new UserChallenge
            {
                UserId = userId,
                Challenge = challenge,
                ChallengeId = challenge.ChallengeId
            };

            if (DateTime.Compare(challenge.StartDateTime, DateTime.Now) <= 0)
            {
                ModelState.AddModelError("StartDateTime", "Date/Time is in the past. Please enter future Date/Time.");
            }
            if (DateTime.Compare(challenge.EndDateTime, challenge.StartDateTime) <= 0)
            {
                ModelState.AddModelError("EndDateTime", "End Date/Time should be after the start Date/Time. Please re-enter Date/Time.");
            }

            if (isAdminOrCoord() && !challenge.IsGroupChallenge)
            {
                ModelState.AddModelError("IsGroupChallenge", "Must be a group challenge");
            }

            if (id != challenge.ChallengeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _challengeRepository.UpdateAsync(challenge);
                    if (!challenge.IsGroupChallenge)
                    {
                        await _userChallengeRepository.AddAsync(user);
                    }
                    //await _context.SaveChangesAsync(); //TODO Tidy
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeExists(challenge.ChallengeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var groupsResponse = await client.GetAsync(_appConfig.GetValue<string>("UserGroupsUrl") + "api/groups");
            var groups = groupsResponse.Content.ReadAsStringAsync().Result;
            var items = GetGroups(groups);

            ViewData["GoalMetricId"] = new SelectList(await _goalMetricRepository.GetAllAsync(), "GoalMetricId", "GoalMetricDisplay");
            ViewData["ActivityId"] = new SelectList(_activityRepository.GetDBSet(), "ActivityId", "ActivityName");
            ViewData["Groupid"] = new SelectList(items, "Value", "Text");

            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _challengeRepository.GetByIdIncAsync((int) id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await _challengeRepository.FindByIdAsync(id);
            await _challengeRepository.DeleteAsync(challenge);
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeExists(int id)
        {
            return _challengeRepository.Exists(id);
        }

        [HttpGet]
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _challengeRepository.FindByIdAsync((int) id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        [HttpPost, ActionName("Join")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinConfirmed(int id)
        {


            var userId = User.Claims.Single(c => c.Type == "sub").Value;
            var challenge = await _challengeRepository.FindByIdAsync(id);
            var userChallenge = await _userChallengeRepository.GetByCid_Uid(userId, id);

            if (userChallenge.Count() > 0) {
                return RedirectToAction(nameof(Index));
            }

            if (challenge == null)
            {
                return NotFound();
            }

            UserChallenge user = new UserChallenge
            {
                UserId = userId,
                Challenge = challenge,
                ChallengeId = challenge.ChallengeId
            };

            await _userChallengeRepository.AddAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public bool isAdminOrCoord()
        {
            return (User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("coordinator") || User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("administrator"));
        }

        public IList<SelectListItem> GetGroups(string groups)
        {
            dynamic data = JsonConvert.DeserializeObject(groups);
            IList<SelectListItem> items = new List<SelectListItem>();
            int i = 0;

            foreach (var d in data)
            {
                i++;
                var dataName = (string)d.name;
                var item = new SelectListItem { Text = dataName, Value = d.id };
                items.Add(item);
            }
            return items;
        }
    }
}
