using System.Collections.Generic;
using challenges.Controllers.api;
using challenges.Models;
using challenges.Repositories;
using challengesTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace challengesTest.Controllers.api
{
    public class ActivitiesControllerAPI_Test
    {
        private readonly ActivitiesController _controller;
        private readonly Mock<IActivityRepository> _activityRepository;

        public ActivitiesControllerAPI_Test()
        {
            _activityRepository = new Mock<IActivityRepository>();
            _controller = new ActivitiesController(_activityRepository.Object);
        }

        [Fact]
        public async void GetsActivities_ReturnsAllActivities()
        {
            var activities = ActivitiesGenerator.Create(5);
            _activityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(activities).Verifiable();
            var result = await _controller.GetActivities();
            Assert.IsType<OkObjectResult>(result);
            _activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetActivities_NoActivities()
        {
            _activityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Activity>) null).Verifiable();
            var result = await _controller.GetActivities();
            Assert.IsType<NotFoundResult>(result);
            _activityRepository.Verify();
            _activityRepository.VerifyNoOtherCalls();
            
        }
        
    }
}