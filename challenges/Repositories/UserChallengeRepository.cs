using System;
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
            return await _context.UserChallenge
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .SingleOrDefaultAsync(g => g.UserChallengeId == id);
        }

        public async Task<List<UserChallenge>> GetAllAsync()
        {
            return await _context.UserChallenge
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .ToListAsync();
        }

        public async Task<List<UserChallenge>> GetByGroupIdAsync(int groupId)
        {
            return await _context.UserChallenge
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .Where(m => m.Challenge.Groupid == groupId)
                .ToListAsync();
        }

        public async Task<List<UserChallenge>> GetAllPersonalChallenges(string userId)
        {
            return await _context.UserChallenge
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .Where(m => m.UserId == userId)
                .Where(m => m.Challenge.IsGroupChallenge == false)
                .ToListAsync();
        }

        public async Task<UserChallenge> AddAsync(UserChallenge userChallenge)
        {
            _context.UserChallenge.Add(userChallenge);
            _context.Entry(userChallenge.Challenge.Activity).State = EntityState.Detached;
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> UpdateAsync(UserChallenge userChallenge)
        {
            userChallenge.Challenge.ActivityId = userChallenge.Challenge.Activity.ActivityId;
            userChallenge.Challenge.Activity = null;
            _context.UserChallenge.Update(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> DeleteAsync(UserChallenge userChallenge)
        {
            _context.Entry(userChallenge.Challenge.Activity).State = EntityState.Detached;
            _context.UserChallenge.Remove(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }
    }
}