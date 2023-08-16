﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context) { }

        public void CreateOnebook(Book book) => Create(book);



        public void DeleteOnebook(Book book) => Delete(book);



        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(b => b.Id).ToListAsync();



        public async Task<Book> GetOneBookIdAsync(int id, bool trackChanges) =>
           await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();


        public void UpdateOnebook(Book book) => Update(book);
    }
}
