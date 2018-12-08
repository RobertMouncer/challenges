using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Data;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly challengesContext _context;

        public ActivityRepository(challengesContext context)
        {
            _context = context;
        }
        
        public async Task<Activity> GetByIdAsync(int id)
        {
            return await _context.Activity.FindAsync(id);
        }

        public async Task<Activity> GetByIdIncAsync(int id)
        {
            return await _context.Activity
                .SingleOrDefaultAsync(g => g.ActivityId == id);
        }

        public async Task<List<Activity>> GetAllAsync()
        {
            return await _context.Activity.ToListAsync();
        }

        public async Task<Activity> AddAsync(Activity activity)
        {
            _context.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> UpdateAsync(Activity activity)
        {
            _context.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> DeleteAsync(Activity activity)
        {
            _context.Remove(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public DbSet<Activity> GetDBSet()
        {
            return _context.Activity;
        }

        public bool Exists(int id)
        {
            return _context.Activity.Any(e => e.ActivityId == id);
        }
        
        public bool Exists(string name)
        {
            return _context.Activity.Any(e => e.ActivityName == name);
        }
    }
}