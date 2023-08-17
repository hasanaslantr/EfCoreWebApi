using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
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

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParametres bookParametres, bool trackChanges)
        {
            var books = await FindAll(trackChanges)
            .FilterBooks(bookParametres.MinPrice, bookParametres.MaxPrice)
            .Search(bookParametres.SearchTerm)
            .OrderBy(b => b.Id)
            
            .ToListAsync();
            return PagedList<Book>
                .ToPagedList(books, bookParametres.PageNumber, bookParametres.PageSize);
        }

        public async Task<Book> GetOneBookIdAsync(int id, bool trackChanges) =>
           await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();


        public void UpdateOnebook(Book book) => Update(book);
    }
}
