using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Data;
using challenges.Models;
using Microsoft.AspNetCore.Authorization;
using challenges.Repositories;
using AberFitnessAuditLogger;

namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc", Policy = "coordinator")]
    public class GoalMetricsController : Controller
    {
        private readonly IGoalMetricRepository _GoalMetricRepository;
        private readonly IAuditLogger auditLogger;
        public GoalMetricsController(IGoalMetricRepository GoalMetricRepository, IAuditLogger auditLogger)
        {
            _GoalMetricRepository = GoalMetricRepository;
            this.auditLogger = auditLogger;
        }

        // GET: GoalMetrics
        public async Task<IActionResult> Index()
        {
            await auditLogger.log(getUserId(), "Accessed Goal Metric Index");
            return View(await _GoalMetricRepository.GetAllAsync());
        }

        // GET: GoalMetrics/Create
        public async Task<IActionResult> CreateAsync()
        {
            await auditLogger.log(getUserId(), "Accessed Goal Metric Create");
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
                goalMetric = await _GoalMetricRepository.AddAsync(goalMetric);
                await auditLogger.log(getUserId(), "Created Goal Metric");
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

            var goalMetric = await _GoalMetricRepository.FindByIdAsync(id);
            if (goalMetric == null)
            {
                return NotFound();
            }
            await auditLogger.log(getUserId(), $"Accessed Edit Goal Metric: {id}");
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
                    goalMetric = await _GoalMetricRepository.UpdateAsync(goalMetric);
                    await auditLogger.log(getUserId(), $"Updated Goal Metric: {goalMetric.GoalMetricId}");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_GoalMetricRepository.GoalMetricExists(goalMetric.GoalMetricId))
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

            var goalMetric = await _GoalMetricRepository.FindByIdAsync(id);
            if (goalMetric == null)
            {
                return NotFound();
            }
            await auditLogger.log(getUserId(), $"Accessed Delete Goal Metric: {id}");
            return View(goalMetric);
        }

        // POST: GoalMetrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goalMetric = await _GoalMetricRepository.FindByIdAsync(id);
            await _GoalMetricRepository.DeleteAsync(goalMetric);
            await auditLogger.log(getUserId(), $"Deleted goal metric: {id}");
            return RedirectToAction(nameof(Index));
        }
        private string getUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
        }

    }
}
