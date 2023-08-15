﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        IQueryable<Book> GetAllBooks(bool trackChanges);
        Book GetOneBookId(int id,bool trackChanges);
        void CreateOnebook(Book book);
        void UpdateOnebook(Book book);
        void DeleteOnebook(Book book);
    }
}
