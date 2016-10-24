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
            throw new NotImplementedException();
        }

        public Contact Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetAll()
        {
            return _cn.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public Contact GetFullContact(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
