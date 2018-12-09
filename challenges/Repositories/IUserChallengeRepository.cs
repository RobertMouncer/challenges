using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenges.Models;

namespace challenges.Repositories
{
    public interface IUserChallengeRepository
    {
        Task<UserChallenge> GetByIdAsync(int id);

        Task<List<UserChallenge>> GetAllAsync();
        
        Task<List<UserChallenge>> GetByGroupIdAsync(string groupId);

        IQueryable<UserChallenge> GetByCid_Uid(string userId, int challengeId);

        Task<List<UserChallenge>> GetAllPersonalChallenges(string userId);

        Task<UserChallenge> AddAsync(UserChallenge userChallenge);

        Task<UserChallenge> UpdateAsync(UserChallenge userChallenge);

        Task<UserChallenge> DeleteAsync(UserChallenge userChallenge);
    }
}