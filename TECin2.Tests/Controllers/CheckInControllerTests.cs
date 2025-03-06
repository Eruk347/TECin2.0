using Azure.Core;
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

        #region Checkin
        [Fact]
        public async Task CheckIn_ShouldReturn200_OKResult_WithCheckInResponse_WithFirstnameHelloMessageAndTurquiseColor_WhenSuccess()
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

        [Fact]
        public async Task CheckIn_ShouldReturn404_NotFound_WhenUserDoesNotExist()
        {
            //Arrange
            _mockCheckInService
                .Setup(service => service.CheckIn(It.IsAny<CheckInRequest>()))
                .ReturnsAsync(() => null);

            CheckInRequest request = new()
            {
                CPR_number = "test",
                CheckinTime = new(),
            };

            //Act
            var result = await _controller.CheckIn(request);

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CheckIn_ShouldReturn500_Problem_WhenExceptionIsRaised()
        {
            //Arrange
            CheckInRequest request = new()
            {
                CPR_number = "test",
                CheckinTime = new(),
            };

            _mockCheckInService
                .Setup(service => service.CheckIn(It.IsAny<CheckInRequest>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));

            //Act
            var result = await _controller.CheckIn(request);

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GetAll_ShouldReturn200_OKResult_WithListOfCheckIns_WhenSuccess()
        {
            //Arrange
            var responses = new List<CheckInResponseLong?> { TestData.TestData.GetCheckInResponseLong(1, "1") };

            _mockCheckInService
                .Setup(service => service.GetAllCheckInStatusesFromGroup(It.IsAny<int>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(responses);

            //Act
            var result = await _controller.GetAll("1,20250303");

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CheckInResponseLong?>>(okResult.Value);
            Assert.Equal(200, statusCodeResult.StatusCode);
            Assert.IsType<List<CheckInResponseLong?>>(returnValue);
        }

        [Fact]
        public async Task GetAll_ShouldReturn204_NoContent_WhenNoCheckIns()
        {
            //Arrange
            var responses = new List<CheckInResponseLong?>();

            _mockCheckInService
                .Setup(service => service.GetAllCheckInStatusesFromGroup(It.IsAny<int>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(responses);

            //Act
            var result = await _controller.GetAll("1,20250303");

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturn500_Problem_WhenServiceReturnsNull()
        {
            //Arrange
            _mockCheckInService
                .Setup(service => service.GetAllCheckInStatusesFromGroup(It.IsAny<int>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _controller.GetAll("1,20250303");

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturn500_Problem_WhenExceptionIsRaised()
        {
            //Arrange
            _mockCheckInService
                .Setup(service => service.GetAllCheckInStatusesFromGroup(It.IsAny<int>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));

            //Act
            var result = await _controller.GetAll("1,20250303");

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
    }
}
