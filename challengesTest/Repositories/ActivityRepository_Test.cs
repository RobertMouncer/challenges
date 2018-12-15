using challenges.Data;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace challengesTest.Repositories
{
    public class ActivityRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<challengesContext> contextOptions;

        public ActivityRepository_Test()
        {
            contextOptions = new DbContextOptionsBuilder<challengesContext>()
                .UseInMemoryDatabase($"rand_db_name_{random.Next()}").Options;
        }

        [Fact]
        public async void GetByIdAsync_ReturnsCorrectItems()
        {
            var list = ActivitiesGenerator.Create(5);
            var expected = list[2];

            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                var activity = await repository.GetByIdIncAsync(expected.ActivityId);
                Assert.IsType<Activity>(activity);
                Assert.Equal(expected, activity);
            }
        }

        [Fact]
        public async void AddAsync_AddsToContext()
        {
            var id = 5;
            var activity = ActivitiesGenerator.CreateActivity(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ActivityRepository(context);
                await repository.AddAsync(activity);
                Assert.Equal(1, await context.Activity.CountAsync());
                Assert.Equal(activity, await context.Activity.SingleAsync());
            }
        }

        [Fact]
        public async void DeleteAsync_RemovesFromContext()
        {
            var id = 5;
            var activity = ActivitiesGenerator.CreateActivity(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.Add(activity);
                context.SaveChanges();
                Assert.Equal(1, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                await repository.DeleteAsync(activity);
                Assert.Equal(0, await context.Activity.CountAsync());
            }
        }

        [Fact]
        public async void UpdateAsync_UpdatesInContext()
        {
            var id = 5;
            var activity = ActivitiesGenerator.CreateActivity(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.Add(activity);
                context.SaveChanges();
                var repository = new ActivityRepository(context);
                var newActivity = await repository.GetByIdIncAsync(activity.ActivityId);
                newActivity.ActivityName = "Push ups";
                await repository.UpdateAsync(newActivity);
                Assert.Equal(1, await context.Activity.CountAsync());
                Assert.Equal(newActivity, await context.Activity.SingleAsync());
            }
        }

        [Fact]
        public async void ActivityExists_checkId()
        {
            var id = 5;
            var activity = ActivitiesGenerator.CreateActivity(id);
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ActivityRepository(context);
                await repository.AddAsync(activity);

                Assert.True(repository.Exists(id));
                Assert.False(repository.Exists(id + 1));
            }

        }

        [Fact]
        public async void ActivityExists_checkName()
        {
            var name = "old hall";
            var activity = ActivitiesGenerator.CreateActivity(5);
            activity.ActivityName = name;
            using (var context = new challengesContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ActivityRepository(context);
                await repository.AddAsync(activity);

                Assert.True(repository.Exists(name));
                Assert.False(repository.Exists(name + " "));
            }
        }
    }
}