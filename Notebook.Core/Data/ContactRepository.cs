using Microsoft.EntityFrameworkCore;
using Notebook.Core.DbContexts;
using Notebook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Core
{
    public class ContactRepository
    {
        private readonly PostgreSQLContext _context;        
        private DomainResult _domainResult;
        public ContactRepository(PostgreSQLContext context) => _context = context;

        public async Task<DomainResult<IEnumerable<Contact>>> GetContactsAsync(
            int userId, int takePages, int skipPages)
        {
            try
            {
                var _contacts = await _context.Contacts.Where(c => c.UserId == userId)
                                                     .Skip(skipPages)
                                                     .Take(takePages).ToListAsync();

                _domainResult = DomainResult<IEnumerable<Contact>>.Success(_contacts);
            }
            catch (Exception ex)
            {
                _domainResult = DomainResult<IEnumerable<Contact>>.Fail(ex.Message);
            }

            return (DomainResult<IEnumerable<Contact>>)_domainResult;
        }

        public async Task<DomainResult> AddContactAsync(int userId, Contact contact)
        {
            var _utcNow = DateTime.UtcNow;

            contact.UserId = userId;
            contact.CreationDate = _utcNow;
            contact.LastEditDate = _utcNow;

            try
            {
                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Success();
            }
            catch (Exception ex)
            {
                _domainResult = DomainResult.Fail(ex.Message);
            }   

            return _domainResult;
        }

        public async Task<DomainResult> EditContactAsync(Contact contact)
        {
            contact.LastEditDate = DateTime.UtcNow;

            try
            {
                _context.Contacts.Update(contact);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Success();
            }
            catch (Exception ex)
            {
                _domainResult = DomainResult.Fail(ex.Message);
            }

            return _domainResult;
        }


        public async Task<DomainResult> DeleteContactAsync(Contact contact)
        {
            try
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Success();
            }
            catch (Exception ex)
            {
                _domainResult = DomainResult.Fail(ex.Message);
            }

            return _domainResult;
        }
    }
}
