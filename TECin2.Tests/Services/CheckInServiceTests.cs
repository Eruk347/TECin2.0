using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Repositories;
using TECin2.API.Services;
using Moq;

namespace TECin2.Tests.Services
{
    public class CheckInServiceTests
    {
        private readonly Mock<ICheckInRepository> _mockCheckInRepository = new();
        private readonly Mock<ISecurityRepository> _mockSecurityRepostitory = new();
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IGroupRepository> _mockGroupRepository = new();
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly CheckInService _checkInservice;

        public CheckInServiceTests()
        {
            _checkInservice = new CheckInService(_mockCheckInRepository.Object, _mockSecurityRepostitory.Object, _mockUserRepository.Object, _mockGroupRepository.Object);
        }

        [Fact]
        public async void
    }
}
