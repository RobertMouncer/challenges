using challenges.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace challengesTest.Controllers.api
{
    public class StatusControllerAPI_Test
    {
        private readonly StatusController _controller;

        public StatusControllerAPI_Test()
        {
            _controller = new StatusController();
        }

        [Fact]
        public void GetStatus_AppRunning()
        {
            var result = _controller.GetStatus();
            Assert.IsType<OkResult>(result);
        }

    }
}