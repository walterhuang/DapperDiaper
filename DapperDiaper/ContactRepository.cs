using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperDiaper.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Transactions;

namespace DapperDiaper
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString);

        public Contact Add(Contact contact)
        {
            var sql =
                "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = _cn.Query<int>(sql, contact).Single();
            contact.Id = id;
            return contact;
        }

        // this won't work!
        public Contact Add2(Contact contact)
        {
            var id = _cn.Query<int>("Contact_Insert", contact, commandType: CommandType.StoredProcedure).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Add3(Contact contact)
        {
            var id = _cn.Query<int>("Contact_Insert",
                new
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Company = contact.Company,
                    Title = contact.Title
                }, commandType: CommandType.StoredProcedure).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Add5(Contact contact)
        {
            var id = _cn.Query<int>("exec Contact_Insert @FirstName, @LastName, @Email, @Company, @Title", contact).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Find(int id)
        {
            return _cn.Query<Contact>(
                "SELECT * FROM Contacts WHERE Id = @Id", new { id }).SingleOrDefault();
        }

        public Contact Find2(int id)
        {
            return _cn.Query<Contact>(
                "Contact_Get", new { Id = id },
                commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Contact> GetAll()
        {
            return _cn.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public void Remove(int id)
        {
            _cn.Execute("DELETE FROM Contacts WHERE Id = @Id", new { Id = id });
        }

        public void Remove2(int id)
        {
            _cn.Execute("Contact_Delete", new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public Contact Update(Contact contact)
        {
            var sql =
                "UPDATE Contacts " +
                "SET FirstName = @FirstName, " +
                "    LastName  = @LastName, " +
                "    Email     = @Email, " +
                "    Company   = @Company, " +
                "    Title     = @Title " +
                "WHERE Id = @Id";
            _cn.Execute(sql, contact);
            return contact;
        }

        public Contact Update2(Contact contact)
        {
            var sql = "Contact_Update";
            _cn.Execute(sql,
                new
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Company = contact.Company,
                    Title = contact.Title
                }, commandType: CommandType.StoredProcedure);
            return contact;
        }

        public void Save(Contact contact)
        {
            using (var txScope = new TransactionScope())
            {
                if (contact.Id == default(int))
                    Add(contact);
                else
                    Update(contact);

                foreach (var addr in contact.Addresses)
                {
                    addr.ContactId = contact.Id;

                    if (addr.Id == default(int))
                        Add(addr);
                    else
                        Update(addr);
                }

                txScope.Complete();
            }
        }

        public Contact GetFullContact(int id)
        {
            var sql =
                "SELECT * FROM Contacts WHERE Id = @Id; " +
                "SELECT * FROM Addresses WHERE ContactId = @Id";

            using (var multipleResults = _cn.QueryMultiple(sql, new { Id = id }))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();

                var addresses = multipleResults.Read<Address>().ToList();
                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }

                return contact;
            }
        }

        public int BulkInsertContacts(List<Contact> contacts)
        {
            var sql =
                "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            return _cn.Execute(sql, contacts);
        }

        #region AddressRepository

        public Address Add(Address address)
        {
            var sql =
                "INSERT INTO Addresses (ContactId, AddressType, StreetAddress, City, StateId, PostalCode) VALUES(@ContactId, @AddressType, @StreetAddress, @City, @StateId, @PostalCode); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = _cn.Query<int>(sql, address).Single();
            address.Id = id;
            return address;
        }

        public Address Update(Address address)
        {
            _cn.Execute("UPDATE Addresses " +
                "SET AddressType = @AddressType, " +
                "    StreetAddress = @StreetAddress, " +
                "    City = @City, " +
                "    StateId = @StateId, " +
                "    PostalCode = @PostalCode " +
                "WHERE Id = @Id", address);
            return address;
        }

        #endregion
    }
}
