using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace challenges.Repositories
{
    public class UserChallengeRepository : IUserChallengeRepository
    {
        private readonly challengesContext _context;

        public UserChallengeRepository(challengesContext context)
        {
            _context = context;
        }

        public async Task<UserChallenge> FindByIdAsync(int id)
        {
            return await _context.UserChallenge.FindAsync(id);
        }

        public async Task<UserChallenge> GetByIdAsync(int id)
        {
            return await _context.UserChallenge
                .Include(c => c.Challenge.GoalMetric)
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .SingleOrDefaultAsync(g => g.UserChallengeId == id);
        }

        public async Task<List<UserChallenge>> GetAllAsync()
        {
            return await _context.UserChallenge
                .Include(c => c.Challenge.GoalMetric)
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .ToListAsync();
        }

        public async Task<List<UserChallenge>> GetByUId(string userId)
        {
            return await _context.UserChallenge.Include(u => u.Challenge).Include(c => c.Challenge.GoalMetric)
                .Include(a => a.Challenge.Activity)
                .Where(c => c.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<List<UserChallenge>> GetGroupByUid(string userId)
        {
            return await _context.UserChallenge
                .Include(c => c.Challenge.GoalMetric)
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .Where(m => m.Challenge.IsGroupChallenge && m.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<UserChallenge>> GetByCid_Uid(string userId, int challengeId)
        {
            return await _context.UserChallenge.Where(uc => uc.ChallengeId == challengeId && uc.UserId == userId).ToListAsync();
        }

        public async Task<List<UserChallenge>> GetAllPersonalChallenges(string userId)
        {
            return await _context.UserChallenge
                .Include(c => c.Challenge.GoalMetric)
                .Include(b => b.Challenge)
                .ThenInclude(c => c.Activity)
                .Where(m => m.UserId == userId)
                .Where(m => m.Challenge.IsGroupChallenge == false)
                .ToListAsync();
        }

        public async Task<UserChallenge> AddAsync(UserChallenge userChallenge)
        {
            _context.UserChallenge.Add(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> UpdateAsync(UserChallenge userChallenge)
        {
            _context.UserChallenge.Update(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public async Task<UserChallenge> DeleteAsync(UserChallenge userChallenge)
        {
            _context.UserChallenge.Remove(userChallenge);
            await _context.SaveChangesAsync();
            return userChallenge;
        }

        public DbSet<UserChallenge> GetDBSet()
        {
            return _context.UserChallenge;
        }

        public bool Exists(int id)
        {
            return _context.UserChallenge.Any(e => e.UserChallengeId == id);
        }
    }
}