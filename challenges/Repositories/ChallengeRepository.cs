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
        private readonly challengesContext _context;
        
        public ChallengeRepository(challengesContext context)
        {
            _context = context;
        }
        
        public async Task<Challenge> GetByIdAsync(int id)
        {
            return await _context.Challenge
                    .Include(b => b.Activity)
                    .SingleOrDefaultAsync(g => g.ChallengeId == id);
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenge
                    .Include(b => b.Activity)
                    .ToListAsync();
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
    }
}