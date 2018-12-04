using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Models;

namespace challenges.Controllers
{
    public class ChallengesController : Controller
    {
        private readonly challengesContext _context;

        public ChallengesController(challengesContext context)
        {
            _context = context;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            var challengesContext = _context.Challenge.Include(c => c.Activity).Include(c => c.UserGroup);
            return View(await challengesContext.ToListAsync());
        }

        // GET: Challenges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .Include(c => c.Activity)
                .Include(c => c.UserGroup)
                .FirstOrDefaultAsync(m => m.ChallengeId == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // GET: Challenges/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityName");
            ViewData["UserGroupId"] = new SelectList(_context.UserGroup, "UserGroupId", "UserGroupId");
            return View();
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChallengeId,StartDateTime,EndDateTime,Goal,PercentageComplete,Repeat,ActivityId,UserGroupId")] Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(challenge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityName", challenge.ActivityId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroup, "UserGroupId", "UserGroupId", challenge.UserGroupId);
            return View(challenge);
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityName", challenge.ActivityId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroup, "UserGroupId", "UserGroupId", challenge.UserGroupId);
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChallengeId,StartDateTime,EndDateTime,Goal,PercentageComplete,Repeat,ActivityId,UserGroupId")] Challenge challenge)
        {
            if (id != challenge.ChallengeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challenge);
                    await _context.SaveChangesAsync();
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
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityName", challenge.ActivityId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroup, "UserGroupId", "UserGroupId", challenge.UserGroupId);
            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .Include(c => c.Activity)
                .Include(c => c.UserGroup)
                .FirstOrDefaultAsync(m => m.ChallengeId == id);
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
            var challenge = await _context.Challenge.FindAsync(id);
            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenge.Any(e => e.ChallengeId == id);
        }
    }
}
