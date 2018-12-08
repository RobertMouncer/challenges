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
using YourApp.Services;
using Newtonsoft.Json;


//simply store info about an activity. Nice and easy.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc", Policy = "coordinator")]
    public class ActivitiesController : Controller
    {
        private readonly challengesContext _context;
        private readonly IApiClient client;

        public ActivitiesController(challengesContext context, IApiClient client)
        {
            _context = context;
            this.client = client;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activity.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
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
                var item = new SelectListItem { Text = dataName, Value = i.ToString() };
                items.Add(item);
            }
            return items;
        }

        // GET: Activities/Create
        public async Task<IActionResult> Create()
        {

            var activities = await client.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/ActivityTypes");
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
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var activities = await client.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/ActivityTypes");
            var activitiesContent = activities.Content.ReadAsStringAsync().Result;
            var items = GetActivities(activitiesContent);

            ViewData["ActivityName"] = new SelectList(items, "Text", "Text");

            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            var activities = await client.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/ActivityTypes");
            var activitiesContent = activities.Content.ReadAsStringAsync().Result;
            var items = GetActivities(activitiesContent);

            ViewData["ActivityName"] = new SelectList(items, "Text", "Text");
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,ActivityName")] Activity activity)
        {
            if (ActivityNameExists(activity.ActivityName))
            {
                ModelState.AddModelError("ActivityName", "Activity already exists. Please enter another activity.");
            }
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityId))
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

            var activities = await client.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/ActivityTypes");
            var activitiesContent = activities.Content.ReadAsStringAsync().Result;
            var items = GetActivities(activitiesContent);

            ViewData["ActivityName"] = new SelectList(items, "Text", "Text");
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.ActivityId == id);
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
            var activity = await _context.Activity.FindAsync(id);
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.ActivityId == id);
        }

        private bool ActivityNameExists(string name)
        {
            return _context.Activity.Any(e => e.ActivityName == name);
        }

    }
}