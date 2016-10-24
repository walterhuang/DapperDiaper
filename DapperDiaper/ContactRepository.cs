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
            _cn.Execute(sql, contact, commandType: CommandType.StoredProcedure);
            return contact;
        }

        public Contact GetFullContact(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
