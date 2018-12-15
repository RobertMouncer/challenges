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
    public class ChallengesManageController_Test
    {
        private readonly Mock<IUserChallengeRepository> _userChallengeRepository;
        private readonly Mock<IChallengeRepository> _challengeRepository;
        private readonly Mock<IActivityRepository> _activityRepository;
        private readonly Mock<IGoalMetricRepository> _goalMetricRepository;
        private readonly IApiClient client;
        private readonly ChallengesManageController controller;
        private readonly IAuditLogger auditLogger;

        public ChallengesManageController_Test()
        {
            _userChallengeRepository = new Mock<IUserChallengeRepository>();
            _challengeRepository = new Mock<IChallengeRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _goalMetricRepository = new Mock<IGoalMetricRepository>();
            var config = new ConfigurationBuilder().Build();
            controller = new ChallengesManageController(_userChallengeRepository.Object, _challengeRepository.Object,_activityRepository.Object,_goalMetricRepository.Object, client, config, auditLogger);
        }

        [Fact]
        public void Index_ShowCorrectView()
        {
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
            //var viewResult = result as ViewResult;
            //Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Index_ContainsCorrectModel()
        {
            var challengesList = ChallengesGenerator.CreateList(5, false);
            var list = new List<Challenge>();
            foreach (UserChallenge uc in challengesList)
            {
                list.Add(uc.Challenge);
            }

            _challengeRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(list).Verifiable();

            var viewResult = await controller.Index() as ViewResult;
            Assert.IsType<List<Challenge>>(viewResult.Model);

            var resources = viewResult.Model as List<Challenge>;
            Assert.Equal(list, resources);
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
        public async void Create_AddsChallenge()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;
            _challengeRepository.Setup(r => r.AddAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.Create(resource);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            _challengeRepository.Verify();
        }

        [Fact]
        public async void Edit_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;

            _challengeRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Edit(1) as ViewResult;
            Assert.IsType<Challenge>(viewResult.Model);

            var resources = viewResult.Model as Challenge;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void edit_UpdatesGoalMetric()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;

            _challengeRepository.Setup(atr => atr.FindByIdAsync(expectedResource.ChallengeId)).ReturnsAsync(expectedResource).Verifiable();

            _challengeRepository.Setup(ar => ar.UpdateAsync(expectedResource)).ReturnsAsync(expectedResource).Verifiable();

            var result = await controller.Edit(expectedResource.ChallengeId);

            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<Challenge>(content.Value);
            Assert.Equal(expectedResource, content.Value);

            _challengeRepository.Verify();
            _challengeRepository.VerifyNoOtherCalls();

        }

        [Fact]
        public async void DeleteChallenge_ReturnsNotFound()
        {
            _challengeRepository.Setup(ar => ar.FindByIdAsync(1)).ReturnsAsync((Challenge)null).Verifiable();

            var result = await controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
            _challengeRepository.Verify();
            _challengeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Delete_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;

            _challengeRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Delete(1) as ViewResult;
            Assert.IsType<Challenge>(viewResult.Model);

            var resources = viewResult.Model as Challenge;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void Delete_ShowsCorrectView()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;
            _challengeRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(resource);
            var result = await controller.Delete(1);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void DeleteConfirmed_DeletesChallenge()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;
            _challengeRepository.Setup(r => r.DeleteAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.DeleteConfirmed(resource.GoalMetricId);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            _challengeRepository.Verify();
        }



        [Fact]
        public async void Join_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge;

            _challengeRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Join(1) as ViewResult;
            Assert.IsType<Challenge>(viewResult.Model);

            var resources = viewResult.Model as Challenge;
            Assert.Equal(expectedResource, resources);
        }
    }
}