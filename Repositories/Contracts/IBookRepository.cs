using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PagedList<Book>> GetAllBooksAsync(BookParametres bookParametres, bool trackChanges); 

        Task<Book> GetOneBookIdAsync(int id,bool trackChanges); 

        void CreateOnebook(Book book);

        void UpdateOnebook(Book book);

        void DeleteOnebook(Book book);
    }
}
