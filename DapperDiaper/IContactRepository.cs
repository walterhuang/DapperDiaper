﻿using DapperDiaper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDiaper
{
    public interface IContactRepository
    {
        Contact Find(int id);
        List<Contact> GetAll();
        Contact Add(Contact contact);
        Contact Update(Contact contact);
        void Remove(int id);

        Contact GetFullContact(int id);
        void Save(Contact contact);
    }
}
