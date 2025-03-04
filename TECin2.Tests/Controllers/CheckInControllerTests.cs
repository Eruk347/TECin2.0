using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using TECin2.API.Controllers;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.Tests.Controllers
{
    public class CheckInControllerTests
    {
        private readonly Mock<ICheckInService> _mockCheckInService;
        private readonly CheckInController _controller;

        public CheckInControllerTests()
        {
            _mockCheckInService = new Mock<ICheckInService>();
            _controller = new CheckInController(_mockCheckInService.Object);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponse_WithFirstnameHelloMessageAndTuquiseColor_WhenSuccess()
        {
            //Arrange
            CheckInRequest request = new()
            {
                CPR_number = "test",
                CheckinTime = new()
            };

            CheckInResponse response = new()
            {
                Color = TestData.TestData.turkis,
                FirstName = "test",
                Message = "test"
            };

            _mockCheckInService
                .Setup(service => service.CheckIn(It.IsAny<CheckInRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _controller.CheckIn(request);

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
    }
}
