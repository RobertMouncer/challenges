using System.Collections.Generic;
using System.Threading.Tasks;
using challenges.Models;

namespace challenges.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> GetByIdAsync(int id);

        Task<List<Activity>> GetAllAsync();

        Task<Activity> AddAsync(Activity activity);

        Task<Activity> UpdateAsync(Activity activity);

        Task<Activity> DeleteAsync(Activity activity);
    }
}