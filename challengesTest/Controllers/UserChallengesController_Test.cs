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
    public class UserChallengesController_Test
    {
        private readonly Mock<IUserChallengeRepository> _userChallengeRepository;
        private readonly Mock<IChallengeRepository> _challengeRepository;
        private readonly IApiClient client;
        private readonly UserChallengesController controller;

        public UserChallengesController_Test()
        {
            _userChallengeRepository = new Mock<IUserChallengeRepository>();
            _challengeRepository = new Mock<IChallengeRepository>();
            var config = new ConfigurationBuilder().Build();
            controller = new UserChallengesController(_userChallengeRepository.Object, _challengeRepository.Object, client, config);
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

            _userChallengeRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(challengesList).Verifiable();

            var viewResult = await controller.Index() as ViewResult;
            Assert.IsType<List<UserChallenge>>(viewResult.Model);

            var resources = viewResult.Model as List<UserChallenge>;
            Assert.Equal(challengesList, resources);
        }
        [Fact]
        public async void Delete_ShowsCorrectView()
        {
            _userChallengeRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ChallengesGenerator.CreateUserChallengePersonal(1));
            var result = await controller.Delete(1);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Delete_ContainsCorrectModel()
        {
            var expectedResource = ChallengesGenerator.CreateUserChallengePersonal(1);
            _userChallengeRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedResource);

            var viewResult = await controller.Delete(1) as ViewResult;
            Assert.IsType<UserChallenge>(viewResult.Model);

            var resources = viewResult.Model as UserChallenge;
            Assert.Equal(expectedResource, resources);
        }

        [Fact]
        public async void DeleteConfirmed_DeletesConfirmed()
        {
            var resource = ChallengesGenerator.CreateUserChallengePersonal(1);
            _userChallengeRepository.Setup(r => r.DeleteAsync(resource)).ReturnsAsync(resource).Verifiable();

            var result = await controller.DeleteConfirmed(resource.UserChallengeId);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectedResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectedResult.ActionName);

            _userChallengeRepository.Verify();
        }

    }
}