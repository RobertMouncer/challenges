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

        public async Task<List<GoalMetric>> GetAllAsync()
        {
            return await _context.GoalMetric.ToListAsync();
        }



    }
}
