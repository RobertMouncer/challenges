using challenges.Controllers.api;
using challenges.Repositories;
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

        public ChallengesControllerAPI_Test()
        {
            _userChallengeRepository = new Mock<IUserChallengeRepository>();
            _challengesRepository = new Mock<IChallengeRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _controller = new ChallengesController(_challengesRepository.Object, _userChallengeRepository.Object,
                _activityRepository.Object);
        }

        [Fact]
        public void GetChallenges_ReturnsAllGroupChallenges()
        {
            //TODO Implement
        }
        
        [Fact]
        public void GetChalleneg_ReturnsAllUserChallenges()
        {
            //TODO Implement
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