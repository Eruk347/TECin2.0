using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;

namespace TECin2.Tests.Repositories
{
    public class CheckInRepositoryTests
    {
        private readonly DbContextOptions<TECinContext> _options;
        private readonly TECinContext _context;
        private readonly CheckInRepository _repository;

        public CheckInRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext>()
                .UseInMemoryDatabase(databaseName: "TECinCheckin")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnListOfCheckIns_WhenSucces()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string userId = "test";
            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = new DateOnly(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
            });
            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = new DateOnly(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteAllCheckInStatusForUser(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Delete_ShouldReturnEmptyList_WhenCheckInDoesNotExistForUser()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteAllCheckInStatusForUser("");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertNewCheckinStatus_ShouldReturnCheckinStatus_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string userId = "test";

            _context.User.Add(TestData.TestData.GetUserTestData(userId));

            CheckInStatus newCheckin = new()
            {
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            };

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertCheckInStatus(newCheckin);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInStatus>(result);
            Assert.Equal(userId, result.User_Id);
            Assert.Equal(newCheckin.ArrivalTime, result.ArrivalTime);
            Assert.Equal(newCheckin.ArrivalDate, result.ArrivalDate);
            Assert.Equal(newCheckin.Departure, result.Departure);
        }

        [Fact]
        public async Task InsertNewDepartment_ShouldReturnNull_WhenIdNumberIsAlreadyUsed()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            CheckInStatus newCheckin = new()
            {
                Id = 1,
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = "test",
                Departure = new(),
            };

            _context.Add(newCheckin);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertCheckInStatus(newCheckin);

            //Assert
            Assert.Null(result);
        }
        #endregion

        #region SelectByUserId
        [Fact]
        public async Task SelectCheckInForUser_ShouldReturnListOfCheckIns_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string userId = "test";

            _context.User.Add(TestData.TestData.GetUserTestData(userId));

            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });

            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectCheckInForUser(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Equal(2, result.Count);

        }

        [Fact]
        public async Task SelectCheckInForUser_ShouldReturnEmptyListOfCheckIns_WhenNoCheckinsExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectCheckInForUser("userId");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }
        #endregion

        #region SelectyByUserIdAndDate
        [Fact]
        public async Task SelectCheckInForUserOnDate_ShouldReturnCheckInStatus_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string userId = "test";
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            _context.User.Add(TestData.TestData.GetUserTestData(userId));
            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = today,
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });
            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = today.AddDays(-1),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });
            await _context.SaveChangesAsync();
            //Act
            var result = await _repository.SelectCheckInForUserOnDate(userId, today);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInStatus>(result);
            Assert.Equal(today, result.ArrivalDate);
        }

        [Fact]
        public async Task SelectCheckInForUserOnDate_ShouldReturnNull_WhenUserDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //Act
            var result = await _repository.SelectCheckInForUserOnDate("userId", today);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SelectCheckInForUserOnDate_ShouldReturnNull_WhenNoCheckInForDateExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string userId = "test";
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            _context.User.Add(TestData.TestData.GetUserTestData(userId));

            //Act
            var result = await _repository.SelectCheckInForUserOnDate("userId", today);

            //Assert
            Assert.Null(result);
        }
        #endregion

        #region SelctByDate
        [Fact]
        public async Task SelectCheckInByDate_ShouldReturnListOfCheckIns_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string userId = "test";
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            _context.User.Add(TestData.TestData.GetUserTestData(userId));

            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = today,
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });

            _context.CheckInStatus.Add(new()
            {
                ArrivalDate = today.AddDays(1),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectCheckInForDate(today);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task SelectCheckInByDate_ShouldReturnEmptyListOfCheckIns_WhenNoCheckinsExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //Act
            var result = await _repository.SelectCheckInForDate(today);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CheckInStatus>>(result);
            Assert.Empty(result);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_ShouldReturnCheckInStatus_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string userId = "test";
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly arrival = TimeOnly.FromDateTime(DateTime.Now);
            TimeOnly departure = TimeOnly.FromDateTime(DateTime.Now.AddHours(2));
            int id = 1;


            _context.User.Add(TestData.TestData.GetUserTestData(userId));

            _context.CheckInStatus.Add(new()
            {
                Id = id,
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = userId,
                Departure = new(),
            });

            CheckInStatus updatedCheckIn = new()
            {
                ArrivalDate = today,
                ArrivalTime = arrival,
                User_Id = userId,
                Departure = departure
            };

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.UpdateCheckInStatus(updatedCheckIn, id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CheckInStatus>(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(updatedCheckIn.User_Id, result.User_Id);
            Assert.Equal(updatedCheckIn.ArrivalDate, result.ArrivalDate);
            Assert.Equal(updatedCheckIn.ArrivalTime, result.ArrivalTime);
            Assert.Equal(updatedCheckIn.Departure, result.Departure);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenCheckInStatusDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            CheckInStatus updatedCheckIn = new()
            {
                ArrivalDate = new(),
                ArrivalTime = new TimeOnly(),
                User_Id = "test",
                Departure = new(),
            };

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.UpdateCheckInStatus(updatedCheckIn, 1);

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
