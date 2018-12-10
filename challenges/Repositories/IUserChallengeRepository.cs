using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Models;
using Microsoft.EntityFrameworkCore;

namespace challenges.Repositories
{
    public interface IUserChallengeRepository
    {
        Task<UserChallenge> FindByIdAsync(int id);
        
        Task<UserChallenge> GetByIdAsync(int id);

        Task<List<UserChallenge>> GetAllAsync();

        IQueryable<UserChallenge> GetAll();

        IQueryable<UserChallenge> GetByUId(string userId);
        
        Task<List<UserChallenge>> GetGroupByUid(string userId);

        IQueryable<UserChallenge> GetByCid_Uid(string userId, int challengeId);

        Task<List<UserChallenge>> GetAllPersonalChallenges(string userId);

        Task<UserChallenge> AddAsync(UserChallenge userChallenge);

        Task<UserChallenge> UpdateAsync(UserChallenge userChallenge);

        Task<UserChallenge> DeleteAsync(UserChallenge userChallenge);

        DbSet<UserChallenge> GetDBSet();

        bool Exists(int id);
    }
}