using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using challenges.Models;
using challenges.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using YourApp.Services;
using Newtonsoft.Json;
using AberFitnessAuditLogger;
using System.Linq;

//simply store info about an activity. Nice and easy.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc", Policy = "coordinator")]
    public class ActivitiesController : Controller
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IApiClient client;
        private readonly IConfigurationSection _appConfig;
        private readonly IAuditLogger auditLogger;

        public ActivitiesController(IActivityRepository activityRepository, IApiClient client, IConfiguration config, IAuditLogger auditLogger)
        {
            _activityRepository = activityRepository;
            this.client = client;
            _appConfig = config.GetSection("Challenges");
            this.auditLogger = auditLogger;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            await auditLogger.log(getUserId(), "Accessed Activity Index");
            return View(await _activityRepository.GetAllAsync());
        }

     
        public IList<SelectListItem> GetActivities(string activitiesContent)
        {
            dynamic data = JsonConvert.DeserializeObject(activitiesContent);
            IList<SelectListItem> items = new List<SelectListItem>();
            int i = 0;

            foreach (var d in data)
            {
                i++;
                var dataName = (string)d.name;
                var dataId = d.Id;
                var item = new SelectListItem { Text = dataName, Value = dataId };
                items.Add(item);
            }
            return items;
        }

        // GET: Activities/Create
        public async Task<IActionResult> Create()
        {
            await auditLogger.log(getUserId(), "Accessed create activity.");
            var activities = await client.GetAsync(_appConfig.GetValue<string>("HealthDataRepositoryUrl") + "api/ActivityTypes");
            var activitiesContent = activities.Content.ReadAsStringAsync().Result;
            var items = GetActivities(activitiesContent);

            ViewData["ActivityName"] = new SelectList(items, "Text", "Text");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,ActivityName")] Activity activity)
        {
            if (ActivityNameExists(activity.ActivityName))
            {
                ModelState.AddModelError("ActivityName", "Activity already exists. Please enter another activity.");
            }

            var activities = await client.GetAsync(_appConfig.GetValue<string>("HealthDataRepositoryUrl") + "api/ActivityTypes");
            var activitiesContent = activities.Content.ReadAsStringAsync().Result;

            activity.DbActivityId = FindDbId(activity.ActivityName, activitiesContent);
            
            if (ModelState.IsValid)
            {
                activity = await _activityRepository.AddAsync(activity);
                await auditLogger.log(getUserId(), $"Created activity: {activity.ActivityName}");
                return RedirectToAction(nameof(Index));
            }

            var items = GetActivities(activitiesContent);
            ViewData["ActivityName"] = new SelectList(items, "Text", "Text");

            return View(activity);
        }

        private int FindDbId(string activityName, string activitiesContent)
        {
            dynamic data = JsonConvert.DeserializeObject(activitiesContent);

            foreach (var d in data)
            {
                var dataName = (string)d.name;
                if (dataName == activityName)
                {
                    return (d.id);
                }
            }
            return 0;
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            await auditLogger.log(getUserId(), $"Accessed Delete activity: {id}");
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _activityRepository.GetByIdIncAsync((int) id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _activityRepository.FindByIdAsync(id);
            await _activityRepository.DeleteAsync(activity);
            await auditLogger.log(getUserId(), $"Deleted activity: {id}");
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _activityRepository.Exists(id);
        }

        private bool ActivityNameExists(string name)
        {
            return _activityRepository.Exists(name);
        }
        private string getUserId()
        {
            return  User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
        }

    }
}