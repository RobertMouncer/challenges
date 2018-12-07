using Microsoft.EntityFrameworkCore;

namespace challenges.Data
{
    public class ChallengesContext : DbContext
    {
        public ChallengesContext (DbContextOptions<ChallengesContext> options)
            : base(options)
        {
        }

        public DbSet<challenges.Models.Activity> Activity { get; set; }

        public DbSet<challenges.Models.Challenge> Challenge { get; set; }

        public DbSet<challenges.Models.UserChallenge> UserChallenge { get; set; }
    }
}
