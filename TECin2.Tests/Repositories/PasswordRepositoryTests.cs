using Microsoft.EntityFrameworkCore;
using Moq;
using TECin2.API.Database;
using TECin2.API.Database.Entities;
using TECin2.API.Repositories;

namespace TECin2.Tests.Repositories
{
    public class PasswordRepositoryTests
    {
        private readonly DbContextOptions<TECinContext2> _options;
        private readonly TECinContext2 _context;
        private readonly PasswordRepository _repository;

        public PasswordRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TECinContext2>()
                .UseInMemoryDatabase(databaseName: "TECinPasswords")
                .Options;

            _context = new(_options);

            _repository = new(_context);
        }

        #region Delete
        [Fact]
        public async Task DeletePassword_ReturnsDeletedPassword()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Password.Add(new Password { Id = "user1", Cipher = "cipher1", Salt = 123 });

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeletePassword("user1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.Id);
            Assert.Equal("cipher1", result.Cipher);
            Assert.Equal(123, result.Salt);
            Assert.IsType<Password>(result);
        }

        [Fact]
        public async Task DeletePassword_ReturnsNull_WhenPasswordNotFound()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _repository.DeletePassword("user1");

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertNewPassword_ReturnsInsertedPassword()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            var password = new Password
            {
                Id = "user1",
                Cipher = "cipher1",
                Salt = 123
            };

            // Act
            var result = await _repository.InsertNewPassword(password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.Id);
            Assert.Equal("cipher1", result.Cipher);
            Assert.Equal(123, result.Salt);
            Assert.IsType<Password>(result);
        }

        [Fact]
        public async Task InsertNewPassword_ReturnsNULL_WhenIdIsUsed()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Password.Add(new Password { Id = "user1", Cipher = "cipher1", Salt = 123 });
            await _context.SaveChangesAsync();
            var password = new Password
            {
                Id = "user1",
                Cipher = "cipher2",
                Salt = 123
            };

            // Act
            var result = await _repository.InsertNewPassword(password);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region Select
        [Fact]
        public async Task SelectPassword_ReturnsPassword()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Password.Add(new Password { Id = "user1", Cipher = "cipher1", Salt = 123 });

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SelectPassword("user1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.Id);
            Assert.Equal("cipher1", result.Cipher);
            Assert.Equal(123, result.Salt);
            Assert.IsType<Password>(result);
        }

        [Fact]
        public async Task SelectPassword_ReturnsNull_WhenPasswordNotFound()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _repository.SelectPassword("user1");

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdatePassword_ReturnsUpdatedPassword()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Password.Add(new Password { Id = "user1", Cipher = "cipher1", Salt = 123 });
            await _context.SaveChangesAsync();
            var updatedPassword = new Password { Id = "user1", Cipher = "newCipher", Salt = 123 };

            // Act
            var result = await _repository.UpdatePassword(updatedPassword);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.Id);
            Assert.Equal("newCipher", result.Cipher);
            Assert.Equal(123, result.Salt);
            Assert.IsType<Password>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNull_WhenPasswordNotFound()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            var updatedPassword = new Password { Id = "user1", Cipher = "newCipher", Salt = 123 };

            // Act
            var result = await _repository.UpdatePassword(updatedPassword);

            // Assert
            Assert.Null(result);
        }
        #endregion
    }
}
