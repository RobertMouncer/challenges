using System.Collections.Generic;
using System.Threading.Tasks;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> GetByIdAsync(int id);

        Task<Activity> GetByIdIncAsync(int id);

        Task<List<Activity>> GetAllAsync();

        Task<Activity> AddAsync(Activity activity);

        Task<Activity> UpdateAsync(Activity activity);

        Task<Activity> DeleteAsync(Activity activity);

        DbSet<Activity> GetDBSet();

        bool Exists(int id);

        bool Exists(string name);
    }
}