using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly challengesContext _context;
        
        public ChallengeRepository(challengesContext context)
        {
            _context = context;
        }
        
        public async Task<Challenge> FindByIdAsync(int id)
        {
            return await _context.Challenge.FindAsync(id);
        }

        public async Task<Challenge> GetByIdIncAsync(int id)
        {
            return await _context.Challenge
                .Include(b => b.Activity).Include(c => c.GoalMetric)
                .SingleOrDefaultAsync(g => g.ChallengeId == id);
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenge
                    .Include(b => b.Activity).Include(c => c.GoalMetric)
                    .ToListAsync();
        }

        public Task<List<Challenge>> GetAllGroup()
        {
            return _context.Challenge.Include(c => c.Activity).Include(c => c.GoalMetric).Where(c => c.IsGroupChallenge).ToListAsync();
        }

        public Task<List<Challenge>> GetAllByGroupId(string id)
        {
            return _context.Challenge.Include(c => c.Activity).Include(c => c.GoalMetric).Where(c => c.IsGroupChallenge && c.Groupid == id).ToListAsync();
        }

        public async Task<Challenge> AddAsync(Challenge challenge)
        {
            _context.Challenge.Add(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> UpdateAsync(Challenge challenge)
        {
            _context.Challenge.Update(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> DeleteAsync(Challenge challenge)
        {
            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public DbSet<Challenge> GetDBSet()
        {
            return _context.Challenge;
        }

        public bool Exists(int id)
        {
            return _context.Challenge.Any(e => e.ChallengeId == id);
        }
    }
}