using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;

namespace TECin2.Tests.Repositories
{
    public class SchoolRepositoryTests
    {
        private readonly DbContextOptions<TECinContext> _options;
        private readonly TECinContext _context;
        private readonly SchoolRepository _repository;

        public SchoolRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext>()
               .UseInMemoryDatabase(databaseName: "TECinSchools")
               .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnDeletedSchool_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int schoolId = 1;
            _context.School.Add(TestData.TestData.GetSchoolTestData(schoolId));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteSchool(schoolId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<School>(result);
            Assert.Equal(schoolId, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturnNull_WhenSchoolDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteSchool(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Insert
        [Fact]
        public async Task Insert_ShouldReturnSchool_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            School newSchool = TestData.TestData.GetSchoolTestData(1);

            //Act
            var result = await _repository.InsertNewSchool(newSchool);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<School>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(newSchool.Name, result.Name);
            Assert.Equal(newSchool.Deactivated, result.Deactivated);
            Assert.Equal(newSchool.Departments, result.Departments);
            Assert.Equal(newSchool.Principal, result.Principal);
        }

        [Fact]
        public async Task Insert_ShouldReturnNull_WhenIdIsAlreadyUsed()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            School newSchool = TestData.TestData.GetSchoolTestData(1);

            _context.Add(newSchool);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertNewSchool(newSchool);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select all
        [Fact]
        public async Task SelectAll_ShouldReturnListOfSchools_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.School.Add(TestData.TestData.GetSchoolTestData(1));
            _context.School.Add(TestData.TestData.GetSchoolTestData(2));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectAllSchools();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<School>>(result);
        }

        [Fact]
        public async Task SelectAll_ShouldReturnEmptyList_WhenNoSchoolsExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectAllSchools();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<School>>(result);
        }
        #endregion


        #region Select by Id
        [Fact]
        public async Task SelectById_ShouldReturnSchool_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int schoolId = 1;

            _context.School.Add(TestData.TestData.GetSchoolTestData(schoolId));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectSchoolById(schoolId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(schoolId, result.Id);
            Assert.IsType<School>(result);
        }

        [Fact]
        public async Task SelectById_ShouldReturnNull_WhenNoSchoolExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectSchoolById(1);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select by name
        [Fact]
        public async Task SelectByName_ShouldReturnSchool_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.School.Add(TestData.TestData.GetSchoolTestData(1));

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectSchoolByName("Test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.IsType<School>(result);
        }

        [Fact]
        public async Task SelectByName_ShouldReturnNull_WhenNoSchoolExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectSchoolByName("");

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Update
        [Fact]
        public async Task Update_ShouldReturnUpdatedSchool_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int schoolId = 1;

            School newSchool = TestData.TestData.GetSchoolTestData(schoolId);
            Department department = TestData.TestData.GetDepartmentTestData();

            _context.Add(newSchool);
            _context.Add(department);
            await _context.SaveChangesAsync();

            School update = new()
            {
                Name = "Test2",
                Deactivated = true,
                Departments = [department],
                Id = schoolId,
                Principal = "2"
            };

            //Act
            var result = await _repository.UpdateSchool(schoolId, update);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<School>(result);
            Assert.Equal(schoolId, result.Id);
            Assert.Equal(update.Name, result.Name);
            Assert.Equal(update.Deactivated, result.Deactivated);
            Assert.Equal(update.Principal, result.Principal);
            Assert.Equal(update.Departments, result.Departments);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenNoSchoolExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            School update = new()
            {
                Name = "Test2",
                Deactivated = true,
                Departments = [],
                Id = 1,
                Principal = "2"
            };

            //Act
            var result = await _repository.UpdateSchool(1, update);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenTryingToAddDepartmentToSchoolThatDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int schoolId = 1;

            School newSchool = TestData.TestData.GetSchoolTestData(schoolId);
            Department department = TestData.TestData.GetDepartmentTestData();

            _context.Add(newSchool);
            await _context.SaveChangesAsync();

            School update = new()
            {
                Name = "Test2",
                Deactivated = true,
                Departments = [department],
                Id = schoolId,
                Principal = "2"
            };

            //Act
            var result = await _repository.UpdateSchool(schoolId, update);

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
