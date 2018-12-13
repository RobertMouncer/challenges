using challenges.Controllers;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;


namespace challengesTest.Controllers
{
    public class GoalMetricController_Test
    {
        private readonly Mock<IGoalMetricRepository> GoalMetricRepository;
        private readonly GoalMetricsController controller;

        public GoalMetricController_Test()
        {
            GoalMetricRepository = new Mock<IGoalMetricRepository>();
            controller = new GoalMetricsController(GoalMetricRepository.Object);
        }

        [Fact]
        public  void Index_ShowCorrectView()
        {
            var result =  controller.Index();
            Assert.IsType<ViewResult>(result);
            //var viewResult = result as ViewResult;
            //Assert.Null(viewResult.ViewName);
        }


        [Fact]
        public async void Index_ContainsCorrectModel()
        {
            var challengesList = ChallengesGenerator.CreateList(5, false);
            var list = new List<GoalMetric>();
            foreach(UserChallenge uc in challengesList)
            {
                list.Add(uc.Challenge.GoalMetric);
            }

            GoalMetricRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(list).Verifiable();

            var viewResult = await controller.Index() as ViewResult;
            Assert.IsType<List<GoalMetric>>(viewResult.Model);

            var resources = viewResult.Model as List<GoalMetric>;
            Assert.Equal(list, resources);
        }

        [Fact]
        public void Create_ShowsCorrectView()
        {
            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Create_AddsGoalMetric()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;
            GoalMetricRepository.Setup(r => r.AddAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.Create(resource);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            GoalMetricRepository.Verify();
        }

        [Fact]
        public async void DeleteActivity_ReturnsNotFound()
        {
            GoalMetricRepository.Setup(ar => ar.FindByIdAsync(1)).ReturnsAsync((GoalMetric)null).Verifiable();

            var result = await controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
            GoalMetricRepository.Verify();
            GoalMetricRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Delete_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;

            GoalMetricRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Delete(1) as ViewResult;
            Assert.IsType<GoalMetric>(viewResult.Model);

            var resources = viewResult.Model as GoalMetric;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void Delete_ShowsCorrectView()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;
            GoalMetricRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(resource);
            var result = await controller.Delete(1);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void DeleteConfirmed_DeletesGoalMetric()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;
            GoalMetricRepository.Setup(r => r.DeleteAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.DeleteConfirmed(resource.GoalMetricId);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            GoalMetricRepository.Verify();
        }

        [Fact]
        public async void Edit_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;

            GoalMetricRepository.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Edit(1) as ViewResult;
            Assert.IsType<GoalMetric>(viewResult.Model);

            var resources = viewResult.Model as GoalMetric;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void edit_UpdatesGoalMetric()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1).Challenge.GoalMetric;

            GoalMetricRepository.Setup(atr => atr.FindByIdAsync(expectedResource.GoalMetricId)).ReturnsAsync(expectedResource).Verifiable();

            GoalMetricRepository.Setup(ar => ar.UpdateAsync(expectedResource)).ReturnsAsync(expectedResource).Verifiable();

            var result = await controller.Edit(expectedResource.GoalMetricId);

            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<Activity>(content.Value);
            Assert.Equal(expectedResource, content.Value);

            GoalMetricRepository.Verify();
            GoalMetricRepository.VerifyNoOtherCalls();

        }
    }
}