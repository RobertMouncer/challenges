using System.Collections.Generic;
using System.Threading.Tasks;
using challenges.Models;

namespace challenges.Repositories
{
    public interface IChallengeRepository
    {
        Task<Challenge> GetByIdAsync(int id);

        Task<List<Challenge>> GetAllAsync();

        Task<List<Challenge>> GetByGroupIdAsync(string groupId);

        Task<Challenge> AddAsync(Challenge challenge);

        Task<Challenge> UpdateAsync(Challenge challenge);

        Task<Challenge> DeleteAsync(Challenge challenge);
    }
}