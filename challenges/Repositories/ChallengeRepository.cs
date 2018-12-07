using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly ChallengesContext _context;
        
        public ChallengeRepository(ChallengesContext context)
        {
            _context = context;
        }
        
        public async Task<Challenge> GetByIdAsync(int id)
        {
            return await _context.Challenge.SingleOrDefaultAsync(g => g.ActivityId == id);
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenge.ToListAsync();
        }

        public async Task<List<Challenge>> GetByGroupIdAsync(string groupId)
        {
            return await _context.Challenge
                .Include(c => c.Activity)
                .Where(m => m.Groupid == groupId)
                .ToListAsync();
        }

        public async Task<Challenge> AddAsync(Challenge challenge)
        {
            _context.Add(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> UpdateAsync(Challenge challenge)
        {
            _context.Update(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> DeleteAsync(Challenge challenge)
        {
            _context.Remove(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }
    }
}