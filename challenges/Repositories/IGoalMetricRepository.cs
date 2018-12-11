using challenges.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Repositories
{
    public interface IGoalMetricRepository
    {
        Task<List<GoalMetric>> GetAllAsync();
    }
}
