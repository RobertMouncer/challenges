using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Models;
using Microsoft.AspNetCore.Authorization;
//display all user challenges.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc")]
    public class UserChallengesController : Controller
    {
        private readonly challengesContext _context;

        public UserChallengesController(challengesContext context)
        {
            _context = context;
        }

        // GET: UserChallenges
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            IQueryable<UserChallenge> challengesContext;

            if (User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("coordinator"))
            {
                 challengesContext = _context.UserChallenge.Include(u => u.Challenge)
                                                                           .Include(a => a.Challenge.Activity)
                                                                           .Where(c => c.UserId.Equals(userId));
            } else
            {
                challengesContext = _context.UserChallenge.Include(u => u.Challenge)
                                                           .Include(a => a.Challenge.Activity)
                                                           .Where(c => c.UserId.Equals(userId));
            }

            return View(await challengesContext.ToListAsync());
        }

        // GET: UserChallenges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChallenge = await _context.UserChallenge
                .Include(u => u.Challenge)
                .FirstOrDefaultAsync(m => m.UserChallengeId == id);
            if (userChallenge == null)
            {
                return NotFound();
            }

            return View(userChallenge);
        }

        // GET: UserChallenges/Create
        public IActionResult Create()
        {
            ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeId");
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
                _context.Add(userChallenge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeId", userChallenge.ChallengeId);
            return View(userChallenge);
        }

        // GET: UserChallenges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChallenge = await _context.UserChallenge.FindAsync(id);
            if (userChallenge == null)
            {
                return NotFound();
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeId", userChallenge.ChallengeId);
            return View(userChallenge);
        }

        // POST: UserChallenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserChallengeId,UserId,ChallengeId,PercentageComplete")] UserChallenge userChallenge)
        {
            if (id != userChallenge.UserChallengeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userChallenge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserChallengeExists(userChallenge.UserChallengeId))
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
            ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeId", userChallenge.ChallengeId);
            return View(userChallenge);
        }

        // GET: UserChallenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChallenge = await _context.UserChallenge
                .Include(u => u.Challenge)
                .FirstOrDefaultAsync(m => m.UserChallengeId == id);
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
            var userChallenge = await _context.UserChallenge.FindAsync(id);
            _context.UserChallenge.Remove(userChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserChallengeExists(int id)
        {
            return _context.UserChallenge.Any(e => e.UserChallengeId == id);
        }
    }
}
