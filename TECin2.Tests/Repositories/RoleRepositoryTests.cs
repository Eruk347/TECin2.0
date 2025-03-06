using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;
using TECin2.Tests.TestData;

namespace TECin2.Tests.Repositories
{
    public class RoleRepositoryTests
    {
        private readonly DbContextOptions<TECinContext> _options;
        private readonly TECinContext _context;
        private readonly RoleRepository _repository;

        public RoleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext>()
                .UseInMemoryDatabase(databaseName: "TECinRoles")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnDeletedRole_WhenSucces()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int roleId = 1;
            _context.Role.Add(new()
            {
                Id = roleId,
                Name = "test",
                Description = "test",
                Deactivated = true,
                Rank = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteRole(roleId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Role>(result);
            Assert.Equal(roleId, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteRole(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Insert
        [Fact]
        public async Task Insert_ShouldReturnRole_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int roleId = 1;
            Role newRole = TestData.TestData.GetRoleTestData(roleId);

            //Act
            var result = await _repository.InsertNewRole(newRole);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Role>(result);
            Assert.Equal(roleId, result.Id);
            Assert.Equal(newRole.Name, result.Name);
            Assert.Equal(newRole.Deactivated, result.Deactivated);
            Assert.Equal(newRole.Description, result.Description);
            Assert.Equal(newRole.Rank, result.Rank);
        }

        [Fact]
        public async Task Insert_ShouldReturnNull_WhenIdNumberIsAlreadyUsed()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Role newRole = TestData.TestData.GetRoleTestData(1);

            _context.Add(newRole);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertNewRole(newRole);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select All
        [Fact]
        public async Task SelectAll_ShouldReturnListOfRoles_WhenRolesExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Role.Add(TestData.TestData.GetRoleTestData(1));
            _context.Role.Add(TestData.TestData.GetRoleTestData(2));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectAlleRoles();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Role>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SelectAll_ShouldReturnAnEmptyList_WhenNoRolesExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectAlleRoles();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Role>>(result);
            Assert.Empty(result);
        }
        #endregion


        #region Select by Id
        [Fact]
        public async Task SelectById_ShouldReturnRole_WhenRoleExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int roleId = 1;
            _context.Role.Add(TestData.TestData.GetRoleTestData(roleId));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectRoleById(roleId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Role>(result);
            Assert.Equal(roleId, result.Id);
        }

        [Fact]
        public async Task SelectById_ShouldReturnNull_WhenRoleDoesNotExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectRoleById(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select by name
        [Fact]
        public async Task SelectByName_ShouldReturnRole_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string name = "Data";
            _context.Role.Add(new()
            {
                Id = 1,
                Name = name,
                Deactivated = false,
                Description = "Data",
                Rank = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectRoleByName(name);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Role>(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task SelectByName_ShouldReturnNull_WhenRoleDoesNotExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectRoleByName("test");

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Update
        [Fact]
        public async Task Update_ShouldReturnUpdatedRole_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int roleId = 1;

            Role role = TestData.TestData.GetRoleTestData(roleId);

            _context.Add(role);
            await _context.SaveChangesAsync();

            Role Update = new()
            {
                Id = roleId,
                Name = "Updated Data",
                Deactivated = true,
                Description = "new",
                Rank = 2
            };

            //Act

            var result = await _repository.UpdateRole(roleId, Update);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Role>(result);
            Assert.Equal(roleId, result.Id);
            Assert.Equal(Update.Name, result.Name);
            Assert.Equal(Update.Deactivated, result.Deactivated);
            Assert.Equal(Update.Description, result.Description);
            Assert.Equal(Update.Rank, result.Rank);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            Role Update = new()
            {
                Id = 1,
                Name = "Updated Data",
                Deactivated = true,
                Description = "new",
                Rank = 2
            };

            //Act

            var result = await _repository.UpdateRole(1, Update);

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
