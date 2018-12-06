using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using challenges.Models;
using Microsoft.AspNetCore.Authorization;
//Index page will be used to display group challenges that users can join. The create function will be used by the userChallenge to create a challenge for the user.
namespace challenges.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc")]
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
            IQueryable<Challenge> challengesContext;
            if (User.Claims.FirstOrDefault(c => c.Type == "user_type").Value.Equals("coordinator"))
            {
                challengesContext =  _context.Challenge.Include(c => c.Activity).Where(c => c.isGroupChallenge);
            }
            else
            {
                //TODO get user group and only display for that group
                challengesContext = _context.Challenge.Include(c => c.Activity).Where(c => c.isGroupChallenge);
            }
            

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
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityId");
            return View();
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChallengeId,StartDateTime,EndDateTime,Goal,Repeat,ActivityId,isGroupChallenge,Groupid")] Challenge challenge)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            if(challenge.Groupid != null)
            {
                challenge.isGroupChallenge = true;
            }
            UserChallenge user = new UserChallenge
            {
                UserId = userId,
                Challenge = challenge,
                ChallengeId = challenge.ChallengeId
            };

            if (ModelState.IsValid)
            {
                
                _context.Add(challenge);
                if (!challenge.isGroupChallenge)
                {
                    _context.Add(user);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "UserChallenges");
            }
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
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityId", challenge.ActivityId);
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChallengeId,StartDateTime,EndDateTime,Goal,Repeat,ActivityId,isGroupChallenge,Groupid")] Challenge challenge)
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
            ViewData["ActivityId"] = new SelectList(_context.Activity, "ActivityId", "ActivityId", challenge.ActivityId);
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

        [HttpGet]
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var challenge = await _context.Challenge.FindAsync(id); ;
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
            var challenge = await _context.Challenge.FindAsync(id);
            var userChallenge = _context.UserChallenge.Where(uc => uc.ChallengeId == id && uc.UserId == userId);
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

            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
