using System.Collections.Generic;
using System.Threading.Tasks;
using challenges.Models;

namespace challenges.Repositories
{
    public interface IUserChallengeRepository
    {
        Task<UserChallenge> GetByIdAsync(int id);

        Task<List<UserChallenge>> GetAllAsync();
        
        Task<List<UserChallenge>> GetByGroupIdAsync(int groupId);

        Task<List<UserChallenge>> GetAllPersonalChallenges(string userId);

        Task<UserChallenge> AddAsync(UserChallenge userChallenge);

        Task<UserChallenge> UpdateAsync(UserChallenge userChallenge);

        Task<UserChallenge> DeleteAsync(UserChallenge userChallenge);
    }
}