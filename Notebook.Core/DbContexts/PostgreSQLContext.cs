using Microsoft.EntityFrameworkCore;
using Notebook.Core.Models;
using System;

namespace Notebook.Core.DbContexts
{
    public class PostgreSQLContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) { }

        public PostgreSQLContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
