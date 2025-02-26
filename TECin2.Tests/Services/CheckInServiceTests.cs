using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Repositories;
using TECin2.API.Services;
using TECin2.API.DTOs;
using Moq;
using TECin2.API.Database.Entities;

namespace TECin2.Tests.Services
{
    public class CheckInServiceTests
    {
        private readonly Mock<ICheckInRepository> _mockCheckInRepository = new();
        private readonly Mock<ISecurityRepository> _mockSecurityRepostitory = new();
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IGroupRepository> _mockGroupRepository = new();
        private readonly CheckInService _checkInservice;

        public CheckInServiceTests()
        {
            _checkInservice = new CheckInService(
                _mockCheckInRepository.Object,
                _mockSecurityRepostitory.Object,
                _mockUserRepository.Object,
                _mockGroupRepository.Object);
        }

        #region CheckIn
        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithHello_WhenSuccessAndFirstCheckIn()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin ==null
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //inserted checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 29, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.InsertCheckInStatus(It.IsAny<CheckInStatus>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 29, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Hej " + user.FirstName, result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithGoodbyeMessage_WhenSuccess_NoMinimumWorkTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            user.Groups.ToList()[0].WorkHoursInDay = new WorkHoursInDay()
            {
                Id = 1,
                Monday = new TimeOnly(0, 30, 0),
                Tuesday = new TimeOnly(0, 30, 0),
                Wednesday = new TimeOnly(0, 30, 0),
                Thursday = new TimeOnly(0, 30, 0),
                Friday = new TimeOnly(0, 30, 0),
            };

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);

            //Update checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user,
                Departure = new(12, 31, 0)
            };

            _mockCheckInRepository
                .Setup(x => x.UpdateCheckInStatus(It.IsAny<CheckInStatus>(), It.IsAny<int>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 31, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Farvel", result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }

        #region NoFlex       
        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithLateMessage_WhenSuccesButLateCheckIn_NoFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin ==null
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //inserted checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.InsertCheckInStatus(It.IsAny<CheckInStatus>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 31, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Du er forsinket", result.Message);
            Assert.Equal(result.Color, TestData.TestData.sennepsGul);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithLeavingEarlyMessage_WhenSuccessButLeavingEarly_NoFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);

            //Update checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user,
                Departure = new(12, 31, 0)
            };

            _mockCheckInRepository
                .Setup(x => x.UpdateCheckInStatus(It.IsAny<CheckInStatus>(), It.IsAny<int>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 31, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Du har først fri", result.Message);
            Assert.Equal(result.Color, TestData.TestData.sennepsGul);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithGoodbyeMessage_WhenSuccessLeavingAfterFullDay_NoFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());


            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);

            //Update checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user,
                Departure = new(15, 01, 0)
            };

            _mockCheckInRepository
                .Setup(x => x.UpdateCheckInStatus(It.IsAny<CheckInStatus>(), It.IsAny<int>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 01, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Farvel", result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }
        #endregion

        #region Flex
        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithHello_WhenSuccesWithinFlexWindow_WithFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            user.Groups.ToList()[0].FlexibleArrivalEnabled = true;
            user.Groups.ToList()[0].FlexibleAmount = new(0, 30);

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin ==null
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //inserted checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 45, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.InsertCheckInStatus(It.IsAny<CheckInStatus>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 45, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Hej", result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithLateMessage_WhenSuccesButLateCheckIn_WithFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups = [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            user.Groups.ToList()[0].FlexibleArrivalEnabled = true;
            user.Groups.ToList()[0].FlexibleAmount = new(0, 30);

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin ==null
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //inserted checkin return
            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(8, 01, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.InsertCheckInStatus(It.IsAny<CheckInStatus>()))
                .ReturnsAsync(checkInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 01, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains(user.Groups.ToList()[0].IsLateMessage, result.Message);
            Assert.Equal(result.Color, TestData.TestData.sennepsGul);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithEarlyMessage_WhenSuccessButLeavingEarly_WithFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups = [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            user.Groups.ToList()[0].FlexibleArrivalEnabled = true;
            user.Groups.ToList()[0].FlexibleAmount = new(0, 30);
            user.Groups.ToList()[0].WorkHoursInDay = new WorkHoursInDay()
            {
                Id = 1,
                Monday = new TimeOnly(7, 0, 0),
                Tuesday = new TimeOnly(7, 0, 0),
                Wednesday = new TimeOnly(7, 0, 0),
                Thursday = new TimeOnly(7, 0, 0),
                Friday = new TimeOnly(6, 30, 0),
            };

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);

            //inserted checkin return
            CheckInStatus updatedCheckInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = firstCheckIn.ArrivalTime,
                Id = firstCheckIn.Id,
                User = user,
                Departure = new(12, 01, 0)
            };

            _mockCheckInRepository
                .Setup(x => x.UpdateCheckInStatus(It.IsAny<CheckInStatus>(), It.IsAny<int>()))
                .ReturnsAsync(updatedCheckInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 01, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("du har først fri", result.Message);
            Assert.Equal(result.Color, TestData.TestData.sennepsGul);
        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithGoodbyeMessage_WhenSuccessLeavingAfterFullDay_WithFlexTime()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups = [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());

            user.Groups.ToList()[0].FlexibleArrivalEnabled = true;
            user.Groups.ToList()[0].FlexibleAmount = new(0, 30);
            user.Groups.ToList()[0].WorkHoursInDay = new WorkHoursInDay()
            {
                Id = 1,
                Monday = new TimeOnly(7, 0, 0),
                Tuesday = new TimeOnly(7, 0, 0),
                Wednesday = new TimeOnly(7, 0, 0),
                Thursday = new TimeOnly(7, 0, 0),
                Friday = new TimeOnly(6, 30, 0),
            };

            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);

            //inserted checkin return
            CheckInStatus updatedCheckInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = firstCheckIn.ArrivalTime,
                Id = firstCheckIn.Id,
                User = user,
                Departure = new(14, 32, 0)
            };

            _mockCheckInRepository
                .Setup(x => x.UpdateCheckInStatus(It.IsAny<CheckInStatus>(), It.IsAny<int>()))
                .ReturnsAsync(updatedCheckInStatus);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 32, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Farvel", result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }

        #region Already checkedIn
        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithAlreadyCheckedInMessage_WhenSuccess()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };

            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);

            //user return
            User user = TestData.TestData.GetUserTestData("1");
            user.Groups ??= [];
            user.Groups.Add(TestData.TestData.GetGroupTestData());


            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            //firstcheckin
            CheckInStatus firstCheckIn = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                User_Id = user.Id,
                ArrivalTime = new(7, 31, 0),
                Id = 1,
                User = user
            };

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(firstCheckIn);


            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 45, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal(result.FirstName, user.FirstName);
            Assert.Contains("Du er allerede tjekket ind", result.Message);
            Assert.Equal(result.Color, TestData.TestData.turkis);
        }
        #endregion

        #region NoUser
        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithNoUserMessage_WhenNoUser()
        {
            //Arrange
            SecurityNumb securityNumber = new()
            {
                Id = "1234",
                Cipher = "1234"
            };
            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(securityNumber);
            //user return
            _mockUserRepository
                .Setup(x => x.SelectUserById(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 45, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal("", result.FirstName);
            Assert.Contains("Lærling findes ikke", result.Message);
            Assert.Equal(result.Color, TestData.TestData.koralRoed);

        }

        [Fact]
        public async Task CheckIn_ShouldReturnCheckInResponseWithNoUserMessage_WhenNoCPRNumber()
        {
            //Arrange            
            _mockSecurityRepostitory
                .Setup(x => x.SelectSecurityNumbByCPR(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Checkinrequest
            CheckInRequest checkInRequest = new()
            {
                CPR_number = "1234",
                CheckinTime = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 45, 0)
            };

            //Act
            var result = await _checkInservice.CheckIn(checkInRequest);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInResponse>(result);
            Assert.Equal("", result.FirstName);
            Assert.Contains("Lærling findes ikke", result.Message);
            Assert.Equal(result.Color, TestData.TestData.koralRoed);

        }
        #endregion
        #endregion
        #endregion

        #region Get all from group
        //success
        [Fact]
        public async Task GetAllCheckInsFromGroup_ShouldReturnListOfCheckInStatuses_WhenSuccess()
        {
            //Arrange
            Group group = TestData.TestData.GetGroupTestData();
            group.Users =
            [
                TestData.TestData.GetUserTestData("1")
            ];
            _mockGroupRepository
                .Setup(x => x.SelectGroupById(It.IsAny<int>()))
                .ReturnsAsync(group);

            List<CheckInStatus> checkInStatuses =
            [
                new CheckInStatus()
                {
                    ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                    ArrivalTime = new(7, 31, 0),
                    User_Id = group.Users.ToList()[0].Id,
                    User = group.Users.ToList()[0]
                }
            ];
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForDate(It.IsAny<DateOnly>()))
                .ReturnsAsync(checkInStatuses);


            CheckInStatus checkInStatus = new()
            {
                ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                ArrivalTime = new(7, 31, 0),
                User_Id = group.Users.ToList()[0].Id,
                User = group.Users.ToList()[0]
            };
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(checkInStatus);

            //Act
            var result = await _checkInservice.GetAllCheckInStatusesFromGroup(group.Id, DateOnly.FromDateTime(DateTime.Now));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInResponseLong>>(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllCheckInsFromGroup_ShouldReturnEmptyList_WhenNoGroupExist()
        {
            //Arrange
            _mockGroupRepository
                .Setup(x => x.SelectGroupById(It.IsAny<int>()))
                .ReturnsAsync(() => null);


            //Act
            var result = await _checkInservice.GetAllCheckInStatusesFromGroup(1, DateOnly.FromDateTime(DateTime.Now));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInResponseLong>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCheckInsFromGroup_ShouldReturnEmptyList_WhenNoUsersInGroup()
        {
            //Arrange
            Group group = TestData.TestData.GetGroupTestData();
            group.Users = null;
            _mockGroupRepository
                .Setup(x => x.SelectGroupById(It.IsAny<int>()))
                .ReturnsAsync(group);
            
            _mockCheckInRepository
                .Setup(x=>x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _checkInservice.GetAllCheckInStatusesFromGroup(1, DateOnly.FromDateTime(DateTime.Now));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInResponseLong>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCheckInsFromGroup_ShouldReturnEmptyList_WhenNoCheckInsForTheDayExist()
        {
            //Arrange
            Group group = TestData.TestData.GetGroupTestData();
            group.Users =
            [
                TestData.TestData.GetUserTestData("1")
            ];
            _mockGroupRepository
                .Setup(x => x.SelectGroupById(It.IsAny<int>()))
                .ReturnsAsync(group);

            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUserOnDate(It.IsAny<string>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _checkInservice.GetAllCheckInStatusesFromGroup(1, DateOnly.FromDateTime(DateTime.Now));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInResponseLong>>(result);
            Assert.Empty(result);
        }
        #endregion

        #region Get all from user
        [Fact]
        public async Task GetAllCheckInsForUser_ShouldReturnListOfCheckInStatuses_WhenSuccess()
        {
            //Arrange
            User user = TestData.TestData.GetUserTestData("1");

            List<CheckInStatus> checkInStatuses =
            [
                new CheckInStatus()
                {
                    ArrivalDate = DateOnly.FromDateTime(DateTime.Now),
                    ArrivalTime = new(7, 31, 0),
                    User_Id = user.Id,
                    User = user
                },
                new CheckInStatus()
                {
                    ArrivalDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                    ArrivalTime = new(7, 31, 0),
                    User_Id = user.Id,
                    User = user
                }
            ];
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUser(It.IsAny<string>()))
                .ReturnsAsync(checkInStatuses);
            //Act
            var result = await _checkInservice.GetAllCheckInStatusesForUser(user.Id);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllCheckInsForUser_ShouldReturnEmptyListOfCheckInStatuses_WhenNoUserExists()
        {
            //Arrange
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUser(It.IsAny<string>()))
                .ReturnsAsync([]);
            //Act
            var result = await _checkInservice.GetAllCheckInStatusesForUser("1");
            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCheckInsForUser_ShouldReturnEmptyListOfCheckInStatuses_WhenNoCheckInsExists()
        {
            //Arrange
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUser(It.IsAny<string>()))
                .ReturnsAsync([]);
            //Act
            var result = await _checkInservice.GetAllCheckInStatusesForUser("1");
            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCheckInsForUser_ShouldReturnEmptyListOfCheckInStatuses_WhenRepositoryReturnNULL()
        {
            //Arrange
            _mockCheckInRepository
                .Setup(x => x.SelectCheckInForUser(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            //Act
            var result = await _checkInservice.GetAllCheckInStatusesForUser("1");
            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }
        #endregion
    }
}
