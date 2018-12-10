using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public interface IChallengeRepository
    {
        Task<Challenge> FindByIdAsync(int id);

        Task<Challenge> GetByIdIncAsync(int id);

        Task<List<Challenge>> GetAllAsync();

        IQueryable<Challenge> GetAllGroup();

        IQueryable<Challenge> GetAllByGroupId(string id);

        Task<Challenge> AddAsync(Challenge challenge);

        Task<Challenge> UpdateAsync(Challenge challenge);

        Task<Challenge> DeleteAsync(Challenge challenge);

        DbSet<Challenge> GetDBSet();

        bool Exists(int id);
    }
}