using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;
using Xunit;

namespace TECin2.Tests.Repositories
{
    public class DepartmentRepositoryTests
    {
        private readonly DbContextOptions<TECinContext> _options;
        private readonly TECinContext _context;
        private readonly DepartmentRepository _repository;

        public DepartmentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext>()
                .UseInMemoryDatabase(databaseName: "TECinDepartments")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnDeletedDepartment_WhenSucces()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int departmentId = 1;
            _context.Department.Add(new()
            {
                Id = departmentId,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteDepartment(departmentId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Department>(result);
            Assert.Equal(departmentId, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteDepartment(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Insert
        [Fact]
        public async Task InsertNewDepartment_ShouldReturnDepartment_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int departmentId = 1;
            int schoolId = 1;
            _context.School.Add(new()
            {
                Id = schoolId,
                Name = "Test School",
                Deactivated = false,
                Principal = null
            });



            Department newDepartment = new()
            {
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = schoolId,
            };

            //Act
            var result = await _repository.InsertNewDepartment(newDepartment);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Department>(result);
            Assert.Equal(departmentId, result.Id);
            Assert.Equal(newDepartment.Name, result.Name);
            Assert.Equal(newDepartment.Deactivated, result.Deactivated);
            Assert.Equal(newDepartment.SchoolId, result.SchoolId);
            Assert.Equal(newDepartment.DepartmentHead, result.DepartmentHead);
        }

        [Fact]
        public async Task InsertNewDepartment_ShouldReturnNull_WhenIdNumberIsAlreadyUsed()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Department newDepartment = new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            };

            _context.Add(newDepartment);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertNewDepartment(newDepartment);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select All
        [Fact]
        public async Task SelectAllDepartments_ShouldReturnListOfDepartments_WhenDepartmentsExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Department.Add(new()
            {
                Id = 1,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "test",
                SchoolId = 1,
            });

            _context.Department.Add(new()
            {
                Id = 2,
                Name = "El",
                Deactivated = false,
                DepartmentHead = "test",
                SchoolId = 1,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectAllDepartments();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Department>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SelectAllDepartments_ShouldReturnAnEmptyList_WhenNoDepartmentsExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectAllDepartments();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Department>>(result);
            Assert.Empty(result);
        }
        #endregion


        #region Select by Id
        [Fact]
        public async Task SelectDepartmentById_ShouldReturnDepartment_WhenDepartmentExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int departmentId = 1;
            int schoolId = 1;
            _context.School.Add(new()
            {
                Id = schoolId,
                Name = "Test School",
                Deactivated = false,
                Principal = null
            });
            _context.Department.Add(new()
            {
                Id = departmentId,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = schoolId,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectDepartmentById(departmentId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Department>(result);
            Assert.Equal(departmentId, result.Id);
        }

        [Fact]
        public async Task SelectDepartmentById_ShouldReturnNull_WhenDepartmentDoesNotExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectDepartmentById(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select by name
        [Fact]
        public async Task SelectDepartmentByName_ShouldReturnDepartment_WhenDepartmentExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string name = "Data";
            _context.Department.Add(new()
            {
                Id = 1,
                Name = name,
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectDepartmentByName(name);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Department>(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task SelectDepartmentByName_ShouldReturnNull_WhenDepartmentDoesNotExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectDepartmentByName("test");

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Update
        [Fact]
        public async Task UpdateDepartment_ShouldReturnUpdatedDepartment_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int departmentId = 1;

            Department newDepartment = new()
            {
                Id = departmentId,
                Name = "Data",
                Deactivated = false,
                DepartmentHead = "testLeader",
                SchoolId = 1,
            };

            _context.Add(newDepartment);
            await _context.SaveChangesAsync();

            Department Update = new()
            {
                Id = departmentId,
                Name = "Updated Data",
                Deactivated = true,
                SchoolId = 2,
                DepartmentHead = "updatedLeader"
            };

            //Act

            var result = await _repository.UpdateDepartment(departmentId, Update);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Department>(result);
            Assert.Equal(departmentId, result.Id);
            Assert.Equal(Update.Name, result.Name);
            Assert.Equal(Update.Deactivated, result.Deactivated);
            Assert.Equal(Update.SchoolId, result.SchoolId);
            Assert.Equal(Update.DepartmentHead, result.DepartmentHead);
        }

        [Fact]
        public async Task UpdateDepartment_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            Department Update = new()
            {
                Id = 1,
                Name = "Updated Data",
                Deactivated = true,
                SchoolId = 2,
                DepartmentHead = "updatedLeader"
            };

            //Act

            var result = await _repository.UpdateDepartment(1, Update);

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
