using challenges.Data;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace challengesTest.Repositories
{
    public class ChallengeRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<challengesContext> contextOptions;

        public ChallengeRepository_Test()
        {
            contextOptions = new DbContextOptionsBuilder<challengesContext>()
                .UseInMemoryDatabase($"rand_db_name_{random.Next()}").Options;
        }

        [Fact]
        public async void FindByIdAsync_ReturnsCorrectItems()
        {
            var list = ChallengesGenerator.CreateList(5, false);
            var expected = list[2];

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(list);
                context.SaveChanges();

                Assert.Equal(list.Count, await context.Challenge.CountAsync());

                var repository = new ChallengeRepository(context);

                var activity = await repository.FindByIdAsync(expected.Challenge.ChallengeId);
                Assert.IsType<Challenge>(activity);
                Assert.Equal(expected.Challenge, activity);
            }
        }

        [Fact]
        public async void getGroups()
        {
            var howManyGroups = 5;
            var list = ChallengesGenerator.CreateList(howManyGroups * 2, true);
            for (int i = 0; i < howManyGroups; i++)
            {
                list[i].Challenge.IsGroupChallenge = false;
            }

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(list);

                context.SaveChanges();

                var repository = new ChallengeRepository(context);
                var groupList = await repository.GetAllGroup();
                Assert.Equal(groupList.Count, howManyGroups);
            }

        }

        [Fact]
        public async void getGroupsByGroupId()
        {
            var howManyGroupsToChange = 5;
            var groupId = "292028903";
            var list = ChallengesGenerator.CreateList(howManyGroupsToChange * 2, true);
            for (int i = 0; i < howManyGroupsToChange; i++)
            {
                list[i].Challenge.Groupid = groupId;
            }

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(list);

                context.SaveChanges();

                var repository = new ChallengeRepository(context);
                var groupList = await repository.GetAllByGroupId(groupId);
                Assert.Equal(groupList.Count, howManyGroupsToChange);
            }

        }

        [Fact]
        public async void AddAsync_AddsToContext()
        {
            var id = 5;
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ChallengeRepository(context);
                await repository.AddAsync(userChallenge.Challenge);

                Assert.Equal(1, await context.Challenge.CountAsync());
                Assert.Equal(userChallenge.Challenge, await context.Challenge.SingleAsync());
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
                context.Challenge.Add(userChallenge.Challenge);
                context.SaveChanges();
                Assert.Equal(1, await context.Activity.CountAsync());
                var repository = new ChallengeRepository(context);
                await repository.DeleteAsync(userChallenge.Challenge);
                Assert.Equal(0, await context.Challenge.CountAsync());
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
                context.Challenge.Add(userChallenge.Challenge);
                context.SaveChanges();
                var repository = new ChallengeRepository(context);
                var newChallenge = await repository.GetByIdIncAsync(userChallenge.Challenge.ChallengeId);
                newChallenge.Goal = 5021;
                await repository.UpdateAsync(newChallenge);
                Assert.Equal(1, await context.Challenge.CountAsync());
                Assert.Equal(newChallenge, await context.Challenge.SingleAsync());
            }
        }

        [Fact]
        public async void ActivityExists_checkid()
        {
            var id = 5;
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(id);

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ChallengeRepository(context);
                await repository.AddAsync(userChallenge.Challenge);

                Assert.True(repository.Exists(userChallenge.Challenge.ChallengeId));
                Assert.False(repository.Exists(userChallenge.Challenge.ChallengeId + 1));
            }
        }
    }
}