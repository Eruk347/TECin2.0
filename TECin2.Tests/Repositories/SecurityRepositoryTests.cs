using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;

namespace TECin2.Tests.Repositories
{
    public class SecurityRepositoryTests
    {
        private readonly DbContextOptions<TECinContext2> _options;
        private readonly TECinContext2 _context;
        private readonly SecurityRepository _repository;

        public SecurityRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext2>()
                .UseInMemoryDatabase(databaseName: "TECinSecurity")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task Delete_ShouldReturnDeletedSecurityNumber_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string userId = "test";
            _context.SecurityNumb.Add(new()
            {
                Cipher = "test",
                Id = userId
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.DeleteSecurityNumb(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SecurityNumb>(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturnNull_WhenSecurityNumberDoesNotExist()
        {
            //Arange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.DeleteSecurityNumb("userId");

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Insert
        [Fact]
        public async Task Insert_ShouldReturnNewSecurityNumber_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string id = "test";

            SecurityNumb securityNumb = new()
            {
                Cipher = "test",
                Id = id
            };

            //Act
            var result = await _repository.InsertNewSecurityNumb(securityNumb);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SecurityNumb>(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task Insert_ShouldReturnNull_WhenTryingToInsertUsedId()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            string id = "test";

            SecurityNumb securityNumb = new()
            {
                Cipher = "test",
                Id = id
            };

            _context.Add(securityNumb);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.InsertNewSecurityNumb(securityNumb);

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select By CPR
        [Fact]
        public async Task SelectByCPR_shouldReturnSecurityNumber_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string cypher = "test";
            string id = "test";

            SecurityNumb securityNumb = new()
            {
                Cipher = cypher,
                Id = id
            };

            _context.Add(securityNumb);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectSecurityNumbByCPR(cypher);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SecurityNumb>(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(cypher, result.Cipher);
        }

        [Fact]
        public async Task SelectByCPR_shouldReturnNull_whenNoSecurityNumberExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            
            //Act
            var result = await _repository.SelectSecurityNumbByCPR("cypher");

            //Assert
            Assert.Null(result);
        }
        #endregion


        #region Select By Id
        [Fact]
        public async Task SelectById_shouldReturnSecurityNumber_WhenSuccess()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            string cypher = "test";
            string id = "test";

            SecurityNumb securityNumb = new()
            {
                Cipher = cypher,
                Id = id
            };

            _context.Add(securityNumb);

            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.SelectSecurityNumbById(id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SecurityNumb>(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(cypher, result.Cipher);
        }

        [Fact]
        public async Task SelectById_shouldReturnNull_whenNoSecurityNumberExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _repository.SelectSecurityNumbById("cypher");

            //Assert
            Assert.Null(result);
        }
        #endregion
    }
}
