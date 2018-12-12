using System.Threading.Tasks;
using challenges.Controllers.api;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YourApp.Services;

namespace challengesTest.Controllers.api
{
    public class ChallengesControllerAPI_Test
    {
        private readonly ChallengesController _controller;
        private readonly Mock<IUserChallengeRepository> _userChallengeRepository;
        private readonly Mock<IChallengeRepository> _challengesRepository;
        private readonly Mock<IActivityRepository> _activityRepository;
        private readonly ApiClient apiClient;

        private string testGroupId = "55";

        public ChallengesControllerAPI_Test()
        {
            _userChallengeRepository = new Mock<IUserChallengeRepository>();
            _challengesRepository = new Mock<IChallengeRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _controller = new ChallengesController(_challengesRepository.Object, _userChallengeRepository.Object,
                _activityRepository.Object, apiClient);
        }

        [Fact]
        public async void ListUserGroupChallenges_ReturnsAllGroupChallenges()
        {
            var challenges = ChallengesGenerator.CreateList(10, true);
            _userChallengeRepository.Setup(r => r.GetGroupByUid(testGroupId)).ReturnsAsync(challenges).Verifiable();
            var result = await _controller.ListUserGroupChallenges(testGroupId);
            Assert.IsType<OkObjectResult>(result);
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void ListPersonalChallenges_ReturnsAllUserChallenges()
        {
            var challenges = ChallengesGenerator.CreateList(10, false);
            _userChallengeRepository.Setup(r => r.GetAllPersonalChallenges("TestUid")).ReturnsAsync(challenges).Verifiable();
            var result = await _controller.ListPersonalChallenges("TestUid");
            Assert.IsType<OkObjectResult>(result);
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void UpdateChallenge_ReturnsBadRequest()
        {
            var userChallenge = ChallengesGenerator.CreateInvalidChallenge();
            _activityRepository.Setup(r => r.FindByIdAsync(3)).ReturnsAsync((Activity) null).Verifiable();
            var result = await _controller.UpdateChallenge(5, userChallenge);
            
            Assert.IsType<BadRequestObjectResult>(result);
            var content = result as BadRequestObjectResult;
            Assert.IsType<SerializableError>(content.Value);
            var errors = content.Value as SerializableError;

            var errorKeys = new[] { "Id", "activityId", "IsGroupChallenge"};

            foreach (var key in errorKeys)
            {
                Assert.True(errors.ContainsKey(key), $"Should have {key} error");
            }
            _activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UpdateChallenge_Updates()
        {
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(2);
            var activity = ActivitiesGenerator.CreateActivity(userChallenge.Challenge.Activity.ActivityId);
            _activityRepository.Setup(c => c.FindByIdAsync(userChallenge.Challenge.Activity.ActivityId)).ReturnsAsync(activity).Verifiable();
            _userChallengeRepository.Setup(uc => uc.UpdateAsync(userChallenge)).ReturnsAsync(userChallenge).Verifiable();

            var result = await _controller.UpdateChallenge(userChallenge.UserChallengeId, userChallenge);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<UserChallenge>(content.Value);
            Assert.Equal(userChallenge, content.Value);
            /*_activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();*/
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void NewChallenge_BadRequestResponse()
        {
            var userChallenge = ChallengesGenerator.CreateInvalidChallenge();
            _activityRepository.Setup(r => r.FindByIdAsync(3)).ReturnsAsync((Activity) null).Verifiable();
            var result = await _controller.NewChallenge(userChallenge);
            
            Assert.IsType<BadRequestObjectResult>(result);
            var content = result as BadRequestObjectResult;
            Assert.IsType<SerializableError>(content.Value);
            var errors = content.Value as SerializableError;

            var errorKeys = new[] {"activityId", "IsGroupChallenge"};

            foreach (var key in errorKeys)
            {
                Assert.True(errors.ContainsKey(key), $"Should have {key} error");
            }
            _activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void NewChallenge_CreatedActivity()
        {
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(2);
            var activity = ActivitiesGenerator.CreateActivity(userChallenge.Challenge.Activity.ActivityId);
            _activityRepository.Setup(a => a.FindByIdAsync(userChallenge.Challenge.Activity.ActivityId)).ReturnsAsync(activity).Verifiable();
            _challengesRepository.Setup(c => c.AddAsync(userChallenge.Challenge)).ReturnsAsync(userChallenge.Challenge).Verifiable();
            _userChallengeRepository.Setup(uc => uc.AddAsync(userChallenge)).ReturnsAsync(userChallenge).Verifiable();

            var result = await _controller.NewChallenge(userChallenge);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<UserChallenge>(content.Value);
            Assert.Equal(userChallenge, content.Value);
            /*_activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();*/
            _challengesRepository.Verify();
            _challengesRepository.VerifyNoOtherCalls();
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteUserChallenge_ReturnsNotFound()
        {
            int id = 1;
            _userChallengeRepository.Setup(uc => uc.GetByIdAsync(1)).ReturnsAsync((UserChallenge)null).Verifiable();

            var result = _controller.DeleteUserChallenge(id);
            Assert.IsType<NotFoundResult>(result.Result);
            
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void DeleteUserChallenge_RemovesChallenge()
        {
            var userChallenge = ChallengesGenerator.CreateUserChallengePersonal(1);
            _userChallengeRepository.Setup(uc => uc.GetByIdAsync(userChallenge.UserChallengeId)).ReturnsAsync(userChallenge).Verifiable();
            _challengesRepository.Setup(uc => uc.DeleteAsync(userChallenge.Challenge)).ReturnsAsync(userChallenge.Challenge).Verifiable();

            var result = await _controller.DeleteUserChallenge(userChallenge.UserChallengeId);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<UserChallenge>(content.Value);
            Assert.Equal(userChallenge, content.Value);
            _challengesRepository.Verify();
            _challengesRepository.VerifyNoOtherCalls();
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }
    }
}