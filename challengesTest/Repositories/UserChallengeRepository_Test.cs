using challenges.Data;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace challengesTest.Repositories
{
    public class UserChallengeRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<challengesContext> contextOptions;

        public UserChallengeRepository_Test()
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

                Assert.Equal(list.Count, await context.UserChallenge.CountAsync());

                var repository = new UserChallengeRepository(context);

                var uc = await repository.FindByIdAsync(expected.UserChallengeId);
                Assert.IsType<UserChallenge>(uc);
                Assert.Equal(expected, uc);
            }
        }

        [Fact]
        public async void GetByIdAsync_ReturnsCorrectItems()
        {
            var list = ChallengesGenerator.CreateList(5, false);
            var expected = list[2];

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(list);
                context.SaveChanges();

                Assert.Equal(list.Count, await context.UserChallenge.CountAsync());

                var repository = new UserChallengeRepository(context);

                var uc = await repository.GetByIdAsync(expected.UserChallengeId);
                Assert.IsType<UserChallenge>(uc);
                Assert.Equal(expected, uc);
            }
        }

        [Fact]
        public async void GetByUserId()
        {
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(5);
            userChallenge.UserId = "ThisIsARandomUserId";
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.Add(userChallenge);
                context.SaveChanges();

                Assert.Equal(1, await context.UserChallenge.CountAsync());

                var repository = new UserChallengeRepository(context);

                var uc = await repository.GetByUId(userChallenge.UserId);
                Assert.IsType<List<UserChallenge>>(uc);
                Assert.Single(uc);
            }
        }

        [Fact]
        public async void GetByGroupUserId()
        {
            var howManyInGroup = 5;
            var userid = "user123";
            var userChallenge = ChallengesGenerator.CreateList(howManyInGroup * 2, true);
            for(int i =0; i< howManyInGroup; i++)
            {
                userChallenge[i].Challenge.IsGroupChallenge = true;
                userChallenge[i].UserId = userid;

            }

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(userChallenge);
                context.SaveChanges();

                Assert.Equal(howManyInGroup * 2, await context.UserChallenge.CountAsync());

                var repository = new UserChallengeRepository(context);

                var uc = await repository.GetGroupByUid(userid);
                Assert.IsType<List<UserChallenge>>(uc);
                Assert.Equal(5,uc.Count);
            }
        }

        [Fact]
        public async void GetByCid_Uid()
        {
            var userid = "user123";
            var userChallenge = ChallengesGenerator.CreateList(10, true);


            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new UserChallengeRepository(context);
                var userChallenges = new List<UserChallenge>();
                foreach (UserChallenge uch in userChallenge)
                {
                    uch.UserId = userid;
                    userChallenges.Add(await repository.AddAsync(uch));
                }
                var expected = userChallenges[2];
                var uc = await repository.GetByCid_Uid(userid, expected.ChallengeId);
                var compare = new List<UserChallenge>();
                compare.Add(expected);
                Assert.IsType<List<UserChallenge>>(uc);
                Assert.Equal(compare, uc);
            }
        }

        [Fact]
        public async void GetAllPersonalChallenges()
        {
            var amount = 10;
            var userid = "user123";
            var userChallenge = ChallengesGenerator.CreateList(amount, false);
            for (int i = 0; i < 10; i++)
            {
                userChallenge[i].UserId = userid;

            }
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.UserChallenge.AddRange(userChallenge);
                context.SaveChanges();

                Assert.Equal(amount, await context.UserChallenge.CountAsync());

                var repository = new UserChallengeRepository(context);

                var uc = await repository.GetAllPersonalChallenges(userid);
                Assert.IsType<List<UserChallenge>>(uc);
                Assert.Equal(amount, uc.Count);
            }
        }
    }
}