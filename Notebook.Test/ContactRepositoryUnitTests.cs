using AutoFixture;
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
    public class ContactRepositoryUnitTests
    {
        private readonly DbContextOptions<PostgreSQLContext> _dbContextOptions;

        public ContactRepositoryUnitTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PostgreSQLContext>().EnableSensitiveDataLogging()
                .UseInMemoryDatabase("ContactsTestDb" + DateTime.Now.ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        [Fact]
        public async void EditContact_Test_Succeess()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _contactRepository = new ContactRepository(_context);

            var _contact = new Fixture().Build<Contact>().Without(c => c.Id).Create();
            await _context.Contacts.AddAsync(_contact);

            var _contactBeforeEdit = new Contact()
            { 
                Number = _contact.Number,
                Name = "new_name",
                Surname = _contact.Surname,
                Description = _contact.Description,
                CreationDate = _contact.CreationDate,
                LastEditDate = _contact.LastEditDate,
                UserId = _contact.UserId
            };

            await _contactRepository.EditContactAsync(_contactBeforeEdit);

            var _contactsAfterEdit = _context.Contacts.ToList();

            Assert.NotNull(_contact);
            Assert.Equal("new_name", _contactsAfterEdit[0].Name = "new_name");
        }


        [Fact]
        public async void DeleteContactAsync_Test_Success()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _contactRepository = new ContactRepository(_context);

            var _contacts = new Fixture().Build<Contact>().Without(c => c.Id).CreateMany(3).ToList();

            await _context.Contacts.AddRangeAsync(_contacts);
            var _contactBeforeDelete = _contacts[0];

            await _contactRepository.DeleteContactAsync(_contactBeforeDelete);
            var _contactsAfterDelete = _context.Contacts;

            Assert.NotNull(_contactsAfterDelete);
            Assert.Equal(2, _contactsAfterDelete.Count());
        }

        [Fact]
        public async void AddContactAsync_Test_Success()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _contactRepository = new ContactRepository(_context);

            var _contact = new Fixture().Build<Contact>().Without(c => c.Id).Create();

            await _contactRepository.AddContactAsync(0, _contact);
            var _contacts = _context.Contacts;

            Assert.NotNull(_contacts);
            Assert.Equal(1, _contacts.Count());
        }

        [Fact]
        public async void GetContactAsync_Test_Success()
        {
            var _context = new PostgreSQLContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _contactRepository = new ContactRepository(_context);

            var _contacts = new Fixture()
                .Build<Contact>()
                .Without(c => c.Id)
                .With(x => x.UserId, 1)
                .CreateMany(3)
                .ToList();

            await _context.Contacts.AddRangeAsync(_contacts);
            await _context.SaveChangesAsync();

            var _domainResult = await _contactRepository.GetContactsAsync(1, int.MaxValue, 0);
            var _contactsAfterGet = _domainResult.Result;

            Assert.True(_domainResult.Succeeded);
            Assert.NotNull(_contactsAfterGet);
            Assert.Equal(3, _contactsAfterGet.Count());
        }
    }
}