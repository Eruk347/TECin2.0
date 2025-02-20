using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using TECin2.API.Controllers;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.Tests.Controllers
{
    public class GroupControllerTests
    {
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly GroupController _controller;

        public GroupControllerTests()
        {
            _mockGroupService = new Mock<IGroupService>();
            _controller = new GroupController(_mockGroupService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfGroups()
        {
            // Arrange
            GroupResponse response = new()
            {
                Id = 1,
                Name = "Group1",
                ArrivalTime = new TimeOnly(8, 0),
                Department = new GroupDepartmentResponse { Id = 1, Name = "Department1" },
            };


            var groups = new List<GroupResponse?> { response };
            _mockGroupService.Setup(service => service.GetAllGroups()).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GroupResponse?>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoGroups()
        {
            // Arrange
            var groups = new List<GroupResponse?>();
            _mockGroupService.Setup(service => service.GetAllGroups()).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithGroup()
        {
            // Arrange
            GroupResponse response = new()
            {
                Id = 1,
                Name = "Group1",
                ArrivalTime = new TimeOnly(8, 0),
                Department = new GroupDepartmentResponse { Id = 1, Name = "Department1" },
            };

            _mockGroupService.Setup(service => service.GetGroupById(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GroupResponse>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetById_Returns500_WhenGroupNotFound()
        {
            // Arrange
            _mockGroupService.Setup(service => service.GetGroupById(1)).ReturnsAsync((GroupResponse?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedGroup()
        {
            // Arrange
            GroupRequest newGroup = new()
            {
                Name = "Group1",
                DepartmentId = 1,
                ArrivalTime = new TimeOnly(8, 0),
            };
            GroupResponse createdGroup = new()
            {
                Id = 1,
                Name = "Group1",
                ArrivalTime = new TimeOnly(8, 0),
                Department = new GroupDepartmentResponse { Id = 1, Name = "Department1", Deactivated = false },

            };
            _mockGroupService
                .Setup(x => x.CreateGroup(It.IsAny<GroupRequest>(), It.IsAny<string>()))
                .ReturnsAsync(createdGroup);

            // Act
            var result = await _controller.Create(newGroup);


            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Create_Returns500_WhenGroupNotCreated()
        {
            // Arrange
            var newGroup = new GroupRequest { Name = "Group1", DepartmentId = 1, Deactivated = false, ArrivalTime = new TimeOnly(8, 0) };
            _mockGroupService.Setup(service => service.CreateGroup(newGroup, It.IsAny<string>())).ReturnsAsync((GroupResponse?)null);

            // Act
            var result = await _controller.Create(newGroup);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(400, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedGroup()
        {
            // Arrange
            var updateGroup = new GroupRequest { Name = "UpdatedGroup", DepartmentId = 1, Deactivated = false, ArrivalTime = new TimeOnly(8, 0) };
            GroupResponse updatedGroup = new()
            {
                Id = 1,
                Name = "Group1",
                ArrivalTime = new TimeOnly(8, 0),
                Department = new GroupDepartmentResponse { Id = 1, Name = "Department1" },
            };
            _mockGroupService.Setup(service => service.UpdateGroup(1, updateGroup, It.IsAny<string>())).ReturnsAsync(updatedGroup);

            // Act
            var result = await _controller.Update(1, updateGroup);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Update_Returns404_WhenGroupNotFound()
        {
            // Arrange
            var updateGroup = new GroupRequest { Name = "UpdatedGroup", DepartmentId = 1, Deactivated = false, ArrivalTime = new TimeOnly(8, 0) };
            _mockGroupService.Setup(service => service.UpdateGroup(1, updateGroup, It.IsAny<string>())).ReturnsAsync((GroupResponse?)null);

            // Act
            var result = await _controller.Update(1, updateGroup);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WithDeletedGroup()
        {
            // Arrange
            GroupResponse deletedGroup = new()
            {
                Id = 1,
                Name = "Group1",
                ArrivalTime = new TimeOnly(8, 0),
                Department = new GroupDepartmentResponse { Id = 1, Name = "Department1" },
            };
            _mockGroupService.Setup(service => service.DeleteGroup(1, 2, It.IsAny<string>())).ReturnsAsync(deletedGroup);

            // Act
            var result = await _controller.Delete("1,2");

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_Return404_WhenGroupNotFound()
        {
            // Arrange
            _mockGroupService.Setup(service => service.DeleteGroup(1, 2, It.IsAny<string>())).ReturnsAsync((GroupResponse?)null);

            // Act
            var result = await _controller.Delete("1,2");

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);
        }
    }
}
