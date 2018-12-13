using challenges.Data;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace challengesTest.Repositories
{
    public class GoalMetricRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<challengesContext> contextOptions;

        public GoalMetricRepository_Test()
        {
            contextOptions = new DbContextOptionsBuilder<challengesContext>()
                .UseInMemoryDatabase($"rand_db_name_{random.Next()}").Options;
        }
        [Fact]
        public async void AddAsync_AddsToContext()
        {
            var id = 5;
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new GoalMetricRepository(context);
                await repository.AddAsync(userChallenge.Challenge.GoalMetric);

                Assert.Equal(1, await context.GoalMetric.CountAsync());
                Assert.Equal(userChallenge.Challenge.GoalMetric, await context.GoalMetric.SingleAsync());
            }
        }

        [Fact]
        public async void DeleteAsync_RemovesFromContext()
        {
            var id = 5;
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.GoalMetric.Add(userChallenge.Challenge.GoalMetric);
                context.SaveChanges();
                Assert.Equal(1, await context.GoalMetric.CountAsync());
                var repository = new GoalMetricRepository(context);
                await repository.DeleteAsync(userChallenge.Challenge.GoalMetric);
                Assert.Equal(0, await context.GoalMetric.CountAsync());
            }
        }

        [Fact]
        public async void UpdateAsync_UpdatesInContext()
        {
            var id = 5;
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();

                var repository = new GoalMetricRepository(context);
                await repository.AddAsync(userChallenge.Challenge.GoalMetric);
                var newGoal = await context.GoalMetric.SingleAsync();

                newGoal.GoalMetricDisplay = "Display me";
                await repository.UpdateAsync(newGoal);

                Assert.Equal(1, await context.GoalMetric.CountAsync());
                Assert.Equal(newGoal, await context.GoalMetric.SingleAsync());
            }
        }

        [Fact]
        public async void FindByIdAsync_ReturnsCorrectItems()
        {
            var list = new List<GoalMetric>();
            for(int i = 0;i < 10; i++)
            {
                list.Add(ChallengesGenerator.CreateUserChallengePersonal(i).Challenge.GoalMetric);
            }
            GoalMetric expected = list[2];
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.GoalMetric.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.GoalMetric.CountAsync());
                var repository = new GoalMetricRepository(context);
                GoalMetric metric = await repository.FindByIdAsync(expected.GoalMetricId);
                Assert.IsType<GoalMetric>(metric);
                Assert.Equal(expected, metric);
            }
        }

    }
}
