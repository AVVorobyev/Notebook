using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Notebook.Core;
using Notebook.Core.DbContexts;
using Notebook.Core.Models;
using System;
using System.Linq;
using Xunit;

namespace Notebook.Test
{
    public class UserRepositoryUnitTests
    {
        private readonly DbContextOptions<PostgreSQLContext> _dbContextOptions;

        public UserRepositoryUnitTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PostgreSQLContext>().EnableSensitiveDataLogging()
                .UseInMemoryDatabase("ContactsTestDb" + DateTime.Now.ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        [Fact]
        public async void AddUserAsync_Test_Success()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _userRepository = new UserRepository(_context);

            var _user = new User()
            {
                UserName = "user_name_111",
                Password = "user_password_111"
            };

            await _userRepository.AddUserAsync(_user);

            Assert.Equal(1, _context.Users.Count());
            Assert.Equal("user_name_111", _context.Users.First().UserName);
        }


        [Fact]
        public async void GetTokenAsync_Success_Test()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var _userRepository = new UserRepository(_context);

            var _user = new User()
            {
                UserName = "user_name_222",
                Password = "user_password_222"
            };

            await _userRepository.AddUserAsync(_user);

            var _authorizingUser = new User()
            {
                UserName = "user_name_222",
                Password = "user_password_222"
            };

            var domainResult = await _userRepository.GetTokenAsync(_authorizingUser);

            var token = domainResult.Result;

            Assert.NotNull(token);      
        }
    }
}
