using DapperDiaper.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDiaper.Tests
{
    public class ContactsDB : DbContext
    {
        public ContactsDB() : base("ContactsDB") { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().MapToStoredProcedures();

            base.OnModelCreating(modelBuilder);
        }
    }
}
