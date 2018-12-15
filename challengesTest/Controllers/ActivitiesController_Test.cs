using AberFitnessAuditLogger;
using challenges.Controllers;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourApp.Services;

namespace challengesTest.Controllers
{
    public class ActivitiesController_Test
    {
        private readonly Mock<IActivityRepository> activityRepository;
        private readonly ApiClient _client;
        private readonly ActivitiesController controller;
        private readonly IAuditLogger auditLogger;

        public ActivitiesController_Test()
        {
            activityRepository = new Mock<IActivityRepository>();
            var config = new ConfigurationBuilder().Build();
            controller = new ActivitiesController(activityRepository.Object, _client, config, auditLogger.Object);

        }

        [Fact]
        public async void Index_ShowCorrectView()
        {
            var result = await controller.Index();
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }


        [Fact]
        public async void Index_ContainsCorrectModel()
        {
            var activities = ActivitiesGenerator.Create(5);
            activityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(activities).Verifiable();

            var viewResult = await controller.Index() as ViewResult;
            Assert.IsType<List<Activity>>(viewResult.Model);

            var resources = viewResult.Model as List<Activity>;
            Assert.Equal(activities, resources);
        }

        [Fact]
        public void Create_ShowsCorrectView()
        {
            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
            //var viewResult = result as ViewResult;
            //Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Create_AddsActivity()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.Activity;
            activityRepository.Setup(r => r.AddAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.Create(resource);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            activityRepository.Verify();
        }


        [Fact]
        public async void DeleteActivity_ReturnsNotFound()
        {
            activityRepository.Setup(ar => ar.GetByIdIncAsync(1)).ReturnsAsync((Activity)null).Verifiable();

            var result = await controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Delete_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.Activity;

            activityRepository.Setup(r => r.GetByIdIncAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Delete(1) as ViewResult;
            Assert.IsType<Activity>(viewResult.Model);

            var resources = viewResult.Model as Activity;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void Delete_ShowsCorrectView()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.Activity;
            activityRepository.Setup(r => r.GetByIdIncAsync(1)).ReturnsAsync(resource);
            var result = await controller.Delete(1);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void DeleteConfirmed_DeletesActivity()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.Activity;
            activityRepository.Setup(r => r.DeleteAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.DeleteConfirmed(resource.ActivityId);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            activityRepository.Verify();
        }
    }
}