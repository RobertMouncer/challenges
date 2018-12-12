using Microsoft.EntityFrameworkCore;
using challenges.Models;

namespace challenges.Data
{
    public class challengesContext : DbContext
    {
        public challengesContext (DbContextOptions<challengesContext> options)
            : base(options)
        {
        }

        public DbSet<challenges.Models.Activity> Activity { get; set; }

        public DbSet<challenges.Models.Challenge> Challenge { get; set; }

        public DbSet<challenges.Models.UserChallenge> UserChallenge { get; set; }

        public DbSet<challenges.Models.GoalMetric> GoalMetric { get; set; }
    }
}
