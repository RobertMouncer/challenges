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
        Task<GoalMetric> AddAsync(GoalMetric goalMetric);
        Task<GoalMetric> FindByIdAsync(int? id);
        Task<GoalMetric> UpdateAsync(GoalMetric goalMetric);
        Task<GoalMetric> DeleteAsync(GoalMetric goalMetric);
        bool GoalMetricExists(int id);
    }
}
