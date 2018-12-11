using challenges.Data;
using challenges.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Repositories
{
    public class GoalMetricRepository : IGoalMetricRepository
    {
        private readonly challengesContext _context;

        public GoalMetricRepository(challengesContext context)
        {
            _context = context;
        }

        public async Task<GoalMetric> AddAsync(GoalMetric goalMetric)
        {
            _context.GoalMetric.Add(goalMetric);
            await _context.SaveChangesAsync();
            return goalMetric;
        }

        public async Task<List<GoalMetric>> GetAllAsync()
        {
            return await _context.GoalMetric.ToListAsync();
        }

        public async Task<GoalMetric> FindByIdAsync(int? id)
        {
            return await _context.GoalMetric.FindAsync(id);
        }
        public async Task<GoalMetric> UpdateAsync(GoalMetric goalMetric)
        {
            _context.GoalMetric.Update(goalMetric);
            await _context.SaveChangesAsync();
            return goalMetric;
        }

        public async Task<GoalMetric> DeleteAsync(GoalMetric goalMetric)
        {
            _context.GoalMetric.Remove(goalMetric);
            await _context.SaveChangesAsync();
            return goalMetric;
        }
        public bool GoalMetricExists(int id)
        {
            return _context.GoalMetric.Any(e => e.GoalMetricId == id);
        }
    }
}
