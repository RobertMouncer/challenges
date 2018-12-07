using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public class UserChallengeRepository : IUserChallengeRepository
    {
        private readonly ChallengesContext _context;

        public UserChallengeRepository(ChallengesContext context)
        {
            _context = context;
        }
        public async Task<UserChallenge> GetByIdAsync(int id)
        {
            return await _context.UserChallenge.SingleOrDefaultAsync(g => g.UserChallengeId == id);
        }

        public async Task<List<UserChallenge>> GetAllAsync()
        {
            return await _context.UserChallenge.ToListAsync();
        }

        public async Task<UserChallenge> AddAsync(UserChallenge userChallenge)
        {
            _context.Add(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> UpdateAsync(UserChallenge userChallenge)
        {
            _context.Update(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> DeleteAsync(UserChallenge userChallenge)
        {
            _context.Remove(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }
    }
}