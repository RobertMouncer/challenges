using challenges.Controllers.api;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace challengesTest.Controllers.api
{
    public class ChallengesControllerAPI_Test
    {
        private readonly ChallengesController _controller;
        private readonly Mock<IUserChallengeRepository> _userChallengeRepository;
        private readonly Mock<IChallengeRepository> _challengesRepository;
        private readonly Mock<IActivityRepository> _activityRepository;

        private string testGroupId = "55";

        public ChallengesControllerAPI_Test()
        {
            _userChallengeRepository = new Mock<IUserChallengeRepository>();
            _challengesRepository = new Mock<IChallengeRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _controller = new ChallengesController(_challengesRepository.Object, _userChallengeRepository.Object,
                _activityRepository.Object);
        }

        [Fact]
        public async void GetChallenges_ReturnsAllGroupChallenges()
        {
            var challenges = ChallengesGenerator.Create(10, true);
            _userChallengeRepository.Setup(r => r.GetByGroupIdAsync("55")).ReturnsAsync(challenges).Verifiable();
            var result = await _controller.ListUserGroupChallenges("55");
            Assert.IsType<OkObjectResult>(result);
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void GetChallenges_ReturnsAllUserChallenges()
        {
            var challenges = ChallengesGenerator.Create(10, false);
            _userChallengeRepository.Setup(r => r.GetAllPersonalChallenges("TestUid")).ReturnsAsync(challenges).Verifiable();
            var result = await _controller.ListPersonalChallenges("TestUid");
            Assert.IsType<OkObjectResult>(result);
            _userChallengeRepository.Verify();
            _userChallengeRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void PutChallenge_ReturnsBadRequest()
        {
            //TODO Implement
        }

        [Fact]
        public void PutChallenges_Updates()
        {
            //TODO Implement
        }

        [Fact]
        public void PostChallenge_CreatedActivity()
        {
            //TODO Implement
        }

        [Fact]
        public void DeleteChallenge_ReturnsNotFound()
        {
            //TODO Implement
        }

        [Fact]
        public void DeleteChallenge_RemovesChallenge()
        {
            //TODO Implement
        }
    }
}