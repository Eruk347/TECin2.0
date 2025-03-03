using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Database.Entities;
using TECin2.API.Database;
using TECin2.API.Repositories;

namespace TECin2.Tests.Repositories
{
    public class GroupRepositoryTests
    {
        private readonly DbContextOptions<TECinContext> _options;
        private readonly TECinContext _context;
        private readonly GroupRepository _repository;

        public GroupRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext>()
                .UseInMemoryDatabase(databaseName: "TECinGroups")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnDeletedGroup_WhenSucces()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int groupId = 1;

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            _context.Group.Add(new()
            {
                Id = groupId,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteGroup(groupId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Group>(result);
            Assert.Equal(groupId, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteGroup(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Insert
        [Fact]
        public async Task InsertNewGroup_ShouldReturnGroup_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            int groupId = 1;

            Group newGroup = new()
            {
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            };

            //Act
            var result = await _repository.InsertNewGroup(newGroup);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Group>(result);
            Assert.Equal(groupId, result.Id);
            Assert.Equal(newGroup.Name, result.Name);
            Assert.Equal(newGroup.Deactivated, result.Deactivated);
            Assert.Equal(newGroup.ArrivalTime, result.ArrivalTime);
            Assert.Equal(newGroup.DepartmentId, result.DepartmentId);
            Assert.Equal(newGroup.WorkHoursInDayId, result.WorkHoursInDayId);
            Assert.Equal(newGroup.FlexibleArrivalEnabled, result.FlexibleArrivalEnabled);
            Assert.Equal(newGroup.FlexibleAmount, result.FlexibleAmount);
            Assert.Equal(newGroup.IsLateBuffer, result.IsLateBuffer);
            Assert.Equal(newGroup.IsLateMessage, result.IsLateMessage);
        }

        [Fact]
        public async Task InsertNewGroup_ShouldReturnNull_WhenIdNumberIsAlreadyUsed()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Group newGroup = new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            };

            _context.Add(newGroup);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertNewGroup(newGroup);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select All
        [Fact]
        public async Task SelectAllGroups_ShouldReturnListOfGroups_WhenGroupsExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            _context.Group.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            });

            _context.Group.Add(new()
            {
                Id = 2,
                Name = "Data2",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectAllGroups();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Group>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SelectAllGroups_ShouldReturnAnEmptyList_WhenNoGroupsExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectAllGroups();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Group>>(result);
            Assert.Empty(result);
        }
        #endregion


        #region Select by Id
        [Fact]
        public async Task SelectGroupById_ShouldReturnGroup_WhenGroupExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            int groupId = 1;
            _context.Group.Add(new()
            {
                Id = groupId,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
               WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectGroupById(groupId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Group>(result);
            Assert.Equal(groupId, result.Id);
        }

        [Fact]
        public async Task SelectGroupById_ShouldReturnNull_WhenGroupDoesNotExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectGroupById(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Update
        [Fact]
        public async Task UpdateGroup_ShouldReturnUpdatedGroup_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            _context.Department.Add(new()
            {
                Id = 2,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            int groupId = 1;

            Group newGroup = new()
            {
                Id = groupId,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            };

            _context.Add(newGroup);
            await _context.SaveChangesAsync();

            Group Update = new()
            {
                Id = 1,
                Name = "Data2",
                Deactivated = true,
                ArrivalTime = TimeOnly.FromDateTime(DateTime.Now),
                DepartmentId = 2,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = false,
                FlexibleAmount = TimeOnly.FromDateTime(DateTime.Now),
                IsLateBuffer = TimeOnly.FromDateTime(DateTime.Now),
                IsLateMessage = "2",
            };

            //Act

            var result = await _repository.UpdateGroup(groupId, Update);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Group>(result);
            Assert.Equal(groupId, result.Id);
            Assert.Equal(newGroup.Name, result.Name);
            Assert.Equal(newGroup.Deactivated, result.Deactivated);
            Assert.Equal(newGroup.ArrivalTime, result.ArrivalTime);
            Assert.Equal(newGroup.DepartmentId, result.DepartmentId);
            Assert.Equal(newGroup.WorkHoursInDayId, result.WorkHoursInDayId);
            Assert.Equal(newGroup.FlexibleArrivalEnabled, result.FlexibleArrivalEnabled);
            Assert.Equal(newGroup.FlexibleAmount, result.FlexibleAmount);
            Assert.Equal(newGroup.IsLateBuffer, result.IsLateBuffer);
            Assert.Equal(newGroup.IsLateMessage, result.IsLateMessage);
        }

        [Fact]
        public async Task UpdateGroup_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            Group Update = new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                ArrivalTime = new(),
                DepartmentId = 1,
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = true,
                FlexibleAmount = new(),
                IsLateBuffer = new(),
                IsLateMessage = "",
            };

            //Act

            var result = await _repository.UpdateGroup(1, Update);

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
