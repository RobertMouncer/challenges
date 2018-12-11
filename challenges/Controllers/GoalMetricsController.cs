using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Data;
using challenges.Models;

namespace challenges.Controllers
{
    public class GoalMetricsController : Controller
    {
        private readonly challengesContext _context;

        public GoalMetricsController(challengesContext context)
        {
            _context = context;
        }

        // GET: GoalMetrics
        public async Task<IActionResult> Index()
        {
            return View(await _context.GoalMetric.ToListAsync());
        }

        // GET: GoalMetrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GoalMetrics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoalMetricId,GoalMetricDisplay,GoalMetricDbName")] GoalMetric goalMetric)
        {
            //TODO CHECK IT DOESN'T ALREADY EXIST
            if (ModelState.IsValid)
            {
                _context.Add(goalMetric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goalMetric);
        }

        // GET: GoalMetrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goalMetric = await _context.GoalMetric.FindAsync(id);
            if (goalMetric == null)
            {
                return NotFound();
            }
            return View(goalMetric);
        }

        // POST: GoalMetrics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoalMetricId,GoalMetricDisplay,GoalMetricDbName")] GoalMetric goalMetric)
        {
            if (id != goalMetric.GoalMetricId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goalMetric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalMetricExists(goalMetric.GoalMetricId))
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
            return View(goalMetric);
        }

        // GET: GoalMetrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goalMetric = await _context.GoalMetric
                .FirstOrDefaultAsync(m => m.GoalMetricId == id);
            if (goalMetric == null)
            {
                return NotFound();
            }

            return View(goalMetric);
        }

        // POST: GoalMetrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goalMetric = await _context.GoalMetric.FindAsync(id);
            _context.GoalMetric.Remove(goalMetric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoalMetricExists(int id)
        {
            return _context.GoalMetric.Any(e => e.GoalMetricId == id);
        }
    }
}
